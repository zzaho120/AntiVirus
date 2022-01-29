﻿using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KrisDevelopment.ERMG
{
	/// <summary>
	/// Add this script to an empty object to snap the end points of the Easy Roads mesh to the current transform
	/// This script is automatically added to all Nav Points generated by ER to do the check and return transform information to the Mesh Gen script.
	/// </summary>
	[AddComponentMenu("Easy Roads Mesh Gen/Point Snap")]
	[ExecuteInEditMode]
	public class ERPointSnap : MonoBehaviour
	{
		public bool passive = false;
		[HideInInspector]
		public Vector3 originalPos = Vector3.zero;
		[HideInInspector]
		public Vector3 originalRot = Vector3.zero;
		[HideInInspector]
		public int snapModeInt = 0;
		[HideInInspector]
		public Transform snappedToPoint;

		public bool snapped { get { return snappedToPoint != null; } }


		private void Update()
		{
			if (snapped)
				UpdatePos();
		}

		private void OnDrawGizmosSelected()
		{
			if (snapped)
				UpdatePos();
		}

		public void UpdatePos()
		{
			if (snappedToPoint == null)
				return;

			UpdateSnappedPosition();
		}

		public void SnapToPointTransform()
		{
			if (passive)
				return;

			originalPos = transform.position;
			originalRot = transform.eulerAngles;

			ERPointSnap[] _waypoints = GameObject.FindObjectsOfType(typeof(ERPointSnap)) as ERPointSnap[];
			Transform _closest = null;
			float _closeDist = Mathf.Infinity;

			foreach (ERPointSnap waypoint in _waypoints)
			{
				float dist = (transform.position - waypoint.transform.position).sqrMagnitude;
				if (dist < _closeDist && waypoint.transform.gameObject != transform.gameObject)
				{
					_closeDist = dist;
					_closest = waypoint.transform;
					snappedToPoint = _closest;
				}
			}

			if (_closest != null)
			{
				snappedToPoint = _closest;
				UpdateSnappedPosition();
				Dirtify();
			}
			else
			{
				Debug.LogError("No Objects Found!");
			}
		}

		private void UpdateSnappedPosition()
		{
			Vector3 _inverseFactor = GetInverseFactor(snapModeInt);
			transform.eulerAngles = snappedToPoint.eulerAngles + _inverseFactor * 180f;
			transform.position = snappedToPoint.transform.position;
		}

		private Vector3 GetInverseFactor(int snapModeInt)
		{
			var _inverseFactor = Vector3.zero;

			if (snapModeInt == 1)
				_inverseFactor = new Vector3(1, 1, 1);
			else if (snapModeInt == 2)
				_inverseFactor = new Vector3(1, 0, 0);
			else if (snapModeInt == 3)
				_inverseFactor = new Vector3(0, 1, 0);
			else if (snapModeInt == 4)
				_inverseFactor = new Vector3(0, 0, 1);
			else if (snapModeInt == 5)
				_inverseFactor = new Vector3(1, 1, 0);
			else if (snapModeInt == 6)
				_inverseFactor = new Vector3(1, 0, 1);
			else if (snapModeInt == 7)
				_inverseFactor = new Vector3(0, 1, 1);

			return _inverseFactor;
		}

		public void UnSnap()
		{
			if (passive)
				return;

			snappedToPoint = null;
			transform.position = originalPos;
			transform.eulerAngles = originalRot;

			Dirtify();
		}

		public void ClearSnap()
		{
			snappedToPoint = null;
			Dirtify();
		}

		private void Dirtify()
		{
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}
	}
}