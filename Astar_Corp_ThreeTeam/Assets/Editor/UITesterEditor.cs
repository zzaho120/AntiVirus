using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WindowDefaultSetting))]
public class UITesterEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		var script = (WindowDefaultSetting)target;

		if (GUILayout.Button("Start with New Window"))
		{
			if (Application.isPlaying)
			{
				Debug.Log("Start with new Window");
				script.StartDoubleWindow();
			}
		}
	}

}
