using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEditorInternal;

[CustomEditor(typeof(WindowManager))]
public class WindowManagerEditor : Editor
{

	private ReorderableList list;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		list.DoLayoutList();

		serializedObject.ApplyModifiedProperties();

		// Bunker Window
		if (GUILayout.Button("Generate Bunker Window Enums"))
        {
			var windows = ((WindowManager)target).windows;
			var total = windows.Length;

			var sb = new StringBuilder();
			sb.Append("public enum BunkerWindows{");
			sb.Append("None,");

			for (var i = 0; i < total; i++)
			{
				sb.Append(windows[i].name.Replace(" ", ""));
				if (i < total - 1)
					sb.Append(",");
			}

			sb.Append("}");

			var path = EditorUtility.SaveFilePanel("Save The Window Enums", "", "BunkerWindowEnums.cs", "cs");

			using (FileStream fs = new FileStream(path, FileMode.Create))
			{

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.Write(sb.ToString());
				}

			}
			AssetDatabase.Refresh();
		}

		// WorldMap Window
		if (GUILayout.Button("Generate WorldMap Window Enums"))
		{
			var windows = ((WindowManager)target).windows;
			var total = windows.Length;

			var sb = new StringBuilder();
			sb.Append("public enum Windows{");
			sb.Append("None,");

			for (var i = 0; i < total; i++)
			{
				sb.Append(windows[i].name.Replace(" ", ""));
				if (i < total - 1)
					sb.Append(",");
			}

			sb.Append("}");

			var path = EditorUtility.SaveFilePanel("Save The Window Enums", "", "WindowEnums.cs", "cs");

			using (FileStream fs = new FileStream(path, FileMode.Create))
			{

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.Write(sb.ToString());
				}

			}
			AssetDatabase.Refresh();
		}

		// Battle Window
		if (GUILayout.Button("Generate BattleMap Window Enums"))
		{
			var windows = ((WindowManager)target).windows;
			var total = windows.Length;

			var sb = new StringBuilder();
			sb.Append("public enum BattleWindows{");
			sb.Append("None,");

			for (var i = 0; i < total; i++)
			{
				sb.Append(windows[i].name.Replace(" ", ""));
				if (i < total - 1)
					sb.Append(",");
			}

			sb.Append("}");

			string path = EditorUtility.SaveFilePanel("Save The Window Enums", "", "BattleWindowEnums.cs", "cs");

			using (FileStream fs = new FileStream(path, FileMode.Create))
			{

				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.Write(sb.ToString());
				}

			}
			AssetDatabase.Refresh();
		}
	}

	private void OnEnable()
	{
		list = new ReorderableList(serializedObject, serializedObject.FindProperty("windows"), true, true, true, true);

		list.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField(rect, "Windows");
		};

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {

			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, Screen.width - 75, EditorGUIUtility.singleLineHeight), element, GUIContent.none);
		};

	}
}
