using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateTile))]
public class GenerateTileEdit : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GenerateTile generator = (GenerateTile)target;
        if (GUILayout.Button("Generate Cubes"))
        {
            generator.Generate();
        }
        if (GUILayout.Button("Generate Building"))
        {
            generator.GenerateBuilding();
        }
    }
}
