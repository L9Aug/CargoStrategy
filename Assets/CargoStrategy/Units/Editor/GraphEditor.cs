using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using CargoStrategy.Units;
using CargoStrategy.Terrain;
using CargoStrategy.Graphing;
using UnityEditor;

public class GraphEditor : EditorWindow
{
    
    [MenuItem("Window/GraphEditor")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GraphEditor));
    }

    public void OnGUI ()
    {
        GameObject[] objects = Selection.gameObjects;
        if (objects.Length == 2 )
        {
            BaseBuilding b1 = objects[0].GetComponent<BaseBuilding>();
            BaseBuilding b2 = objects[1].GetComponent<BaseBuilding>();
            if (b1 != null && b2 != null)
            {
                if (GUILayout.Button("Connect Selected"))
                {
                    RoadMesh m_roadPrefab = AssetDatabase.LoadAssetAtPath<RoadMesh>("assets/CargoStrategy/Terrain/Prefabs/RoadPrefab.prefab");
                    RoadMesh mesh = (RoadMesh)PrefabUtility.InstantiatePrefab(m_roadPrefab);
                    Undo.RecordObject(b1, "connectb1");

                    b1.AddConnection(b2);
                    Undo.RecordObject(b2, "connectb1");

                    b2.AddConnection(b1);

                    mesh.m_to = b1;
                    mesh.m_from = b2;

                    b1.Connections.Add(mesh);
                    b2.Connections.Add(mesh);
                    EditorUtility.SetDirty(b1);
                    EditorUtility.SetDirty(b2);
                }
            }
        }

    }
}
#endif