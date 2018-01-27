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

    public void OnGUI () {

        GameObject[] objects = Selection.gameObjects;
        if (objects.Length == 2 && GraphManager.Instance.RoadPrefab != null)
        {
            BaseBuilding b1 = objects[0].GetComponent<BaseBuilding>();
            BaseBuilding b2 = objects[1].GetComponent<BaseBuilding>();
            if (b1 != null && b2 != null)
            {
                if (GUILayout.Button("Connect Selected"))
                {
                    RoadMesh m_roadPrefab = GraphManager.Instance.RoadPrefab;
                    Undo.RecordObject(b1, "connect");
                    b1.AddConnection(b2);

                    Undo.RecordObject(b2, "connect");
                    b2.AddConnection(b1);
                    RoadMesh mesh = Instantiate(m_roadPrefab);
                    Undo.RecordObject(mesh, "connect");
                    mesh.m_to = b1;
                    mesh.m_from = b2;
                }
            }
        }

    }
}
#endif