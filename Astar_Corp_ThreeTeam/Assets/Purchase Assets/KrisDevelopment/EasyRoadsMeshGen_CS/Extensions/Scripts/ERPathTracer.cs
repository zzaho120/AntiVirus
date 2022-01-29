using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KrisDevelopment.ERMG
{
	[
		AddComponentMenu("Easy Roads Mesh Gen/Extensions/Path Tracer"),
		ExecuteInEditMode
	]
	public class ERPathTracer : MonoBehaviour
	{
		public System.Action onUpdatePoints;

		public ERMeshGen meshGen = null;

		public float distanceRecord { get; private set; }

		[System.NonSerialized]
		private bool subscribedToMeshGen = false;
		PointData[][] pathGroup;

		//accessors:
		private bool validLinking
		{
			get
			{
				Link();
				UpdateSubscription();
				//add reference conditions here
				return (meshGen != null);
			}
		}
		//--

		private void Link()
		{
			if (!meshGen)
				meshGen = GetComponent<ERMeshGen>();
		}

		public void SubToMeshGen(ERMeshGen meshGen)
		{
			meshGen.onUpdateMesh -= OnUpdatePoints;
			meshGen.onUpdateMesh += OnUpdatePoints;
			subscribedToMeshGen = true;
		}

		///<summary> Unsubscribes the OnUpdatePoints delegate from the mesh gen update event </summary>
		public void UnsubFromMeshGen()
		{
			if (meshGen)
				meshGen.onUpdateMesh -= OnUpdatePoints;
			subscribedToMeshGen = false;
		}

		void UpdateSubscription()
		{
			if (meshGen && !subscribedToMeshGen)
				SubToMeshGen(meshGen);

			if (subscribedToMeshGen && (meshGen == null))
				UnsubFromMeshGen();
		}

		void Update()
		{
			if (!validLinking)
				return;

			if (!subscribedToMeshGen)
				OnUpdatePoints();
		}

		void OnUpdatePoints()
		{
			if (onUpdatePoints != null)
				onUpdatePoints();
		}

		void CalculateDistances(ref PointData[] points)
		{
			if (points == null)
				return;

			if (points.Length > 0)
				points[0].distance = 0f;

			float _distance = 0f;

			if (points.Length > 1)
			{
				for (int i = 1; i < points.Length; i++)
				{
					_distance += Vector3.Distance(points[i].position, points[i - 1].position);
					points[i].distance = _distance;
				}
			}

			distanceRecord = _distance;
		}

		OrientationData OrientationDataAtDistance(int pathIndex, float distance)
		{
			int
				_pointIndex = 0;

			for (int i = 0; i < pathGroup[pathIndex].Length - 1; i++)
				if (pathGroup[pathIndex][i].distance < distance)
					_pointIndex = i;
				else break;

			float
				_distanceInterval = Mathf.Abs(pathGroup[pathIndex][_pointIndex + 1].distance - pathGroup[pathIndex][_pointIndex].distance),
				_lerp = (distance - pathGroup[pathIndex][_pointIndex].distance) / _distanceInterval;

			OrientationData _o = new OrientationData(
				Vector3.Lerp(pathGroup[pathIndex][_pointIndex].position, pathGroup[pathIndex][_pointIndex + 1].position, _lerp),
				pathGroup[pathIndex][_pointIndex].tangentVector,
				Vector3.Lerp(pathGroup[pathIndex][_pointIndex].normalVector, pathGroup[pathIndex][_pointIndex + 1].normalVector, _lerp));
			return _o;
		}

		public void TracePath(int pathIndex, float horizontalOffset, float verticalOffset)
		{
			if (!validLinking)
			{
				Debug.LogError("[ERPathTracer.TracePath ERROR] No path source!");
				return;
			}

			//perform initialization if needed
			if (pathGroup == null)
				pathGroup = new PointData[pathIndex][];
				
			if (pathGroup.Length <= pathIndex)
			{
				SETUtil.Deprecated.ArrUtil.Resize<PointData[]>(ref pathGroup, pathIndex - pathGroup.Length + 1);
			}

			pathGroup[pathIndex] = meshGen.GetOrientedPathPoints(horizontalOffset, verticalOffset).ToPointDataArray();
			CalculateDistances(ref pathGroup[pathIndex]);
		}

		public OrientationData Evaluate(int pathIndex, float distance, float elementLength)
		{
			OrientationData _o = new OrientationData(transform.position + Vector3.forward * distance, Vector3.forward, Vector3.up);

			if (pathGroup == null || pathGroup.Length < pathIndex + 1 || pathGroup[pathIndex] == null)
			{
				Debug.LogError("[ERPathTracer.Evaluate ERROR] no path at index " + pathIndex + ". Make sure you trace the path first!");
				return _o;
			}

			//if there is just a single point
			if (pathGroup[pathIndex].Length == 1)
			{
				_o = new OrientationData(transform.position + pathGroup[pathIndex][0].tangentVector * distance, pathGroup[pathIndex][0].tangentVector, pathGroup[pathIndex][0].normalVector);
			}
			else if (pathGroup[pathIndex].Length > 1)
			{ //if there is more than one point
				OrientationData
					_pointA = OrientationDataAtDistance(pathIndex, distance),
					_pointB = OrientationDataAtDistance(pathIndex, distance + elementLength);
				_o.Set(_pointA.position, (_pointB.position - _pointA.position).normalized, _pointA.normalVector);
			}

			return _o;
		}
	}
}
