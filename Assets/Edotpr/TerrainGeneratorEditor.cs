using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor {
    public override void OnInspectorGUI()
    {
        TerrainGenerator tg = (TerrainGenerator)target;

        if (DrawDefaultInspector())
        {
            tg.GenerateTerrain();
        }

        if (GUILayout.Button("Generate"))
        {
            tg.GenerateTerrain();
        }
    }
}
