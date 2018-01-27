using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CargoStrategy.Terrain
{


    public class TerrainMesh : MonoBehaviour {

        [SerializeField]
        private Texture2D m_heightMap = null;

        [SerializeField]
        private MeshFilter m_meshRenderer = null;

        [SerializeField]
        private MeshCollider m_collisionMesh = null;

        [SerializeField]
        private float m_heightScale = 1.0f;

        private Mesh m_mesh = null;

        public void BuildMesh()
        {
            if (m_heightMap != null)
            {
                if (m_mesh == null)
                {
                    m_mesh = new Mesh();
                }
                int width = m_heightMap.width;
                int height = m_heightMap.height;
                List<Vector3> vertices = new List<Vector3>();
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        vertices.Add(new Vector3(x, m_heightMap.GetPixel(x,y).grayscale * m_heightScale, y));
                    }
                }
                List<int> indicies = new List<int>();
                for (int x = 0; x < width - 1; x++)
                {
                    for (int y = 0; y < height - 1; y++)
                    {
                        indicies.Add(x + (y * width));
                        indicies.Add(x + 1 + ((y + 1) * width));
                        indicies.Add(x + ((y + 1) * width));

                        indicies.Add(x + (y * width));
                        indicies.Add(x + 1 + (y * width));
                        indicies.Add(x + 1 + ((y + 1) * width));
                    }
                }
                m_mesh.SetVertices(vertices);
                m_mesh.SetIndices(indicies.ToArray(), MeshTopology.Triangles, 0);
                m_mesh.RecalculateNormals();
            }
            if (m_meshRenderer != null)
            {
                m_meshRenderer.sharedMesh = m_mesh;
            }
            if (m_collisionMesh != null)
            {
                m_collisionMesh.sharedMesh = m_mesh;
            }
        }

        public float GetHeight(Vector3 position)
        {
            if (m_heightMap != null)
            {
                return m_heightMap.GetPixelBilinear(position.x / m_heightMap.width, position.y / m_heightMap.height).grayscale * m_heightScale;
            }
            return 0.0f;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TerrainMesh))]
    public class TerrainMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TerrainMesh mesh = target as TerrainMesh;
            if (mesh != null)
            {
                if (GUILayout.Button("Build Mesh"))
                {
                    mesh.BuildMesh();
                }
            }
        }
    }
#endif

}
