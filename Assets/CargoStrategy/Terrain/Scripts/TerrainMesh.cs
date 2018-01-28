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

        [SerializeField]
        private float m_roadWidth = 7.5f;

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
                List<Vector3> verticies = new List<Vector3>();
                List<Vector2> uv = new List<Vector2>();

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        verticies.Add(new Vector3(x, m_heightMap.GetPixel(x,y).grayscale * m_heightScale, y));
                        uv.Add(new Vector2(((float)x) / width, ((float)y) / height));
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
                RoadMesh[] roads = FindObjectsOfType<RoadMesh>();
                for (int i = 0; i < roads.Length; i++)
                {
                    FlattenLine(verticies, roads[i].From.Position, roads[i].To.Position, m_roadWidth);
                }
                m_mesh.SetVertices(verticies);
                m_mesh.SetUVs(0, uv);
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
                return m_heightMap.GetPixelBilinear((position.x / m_heightMap.width), (position.z / m_heightMap.height)).grayscale * m_heightScale;
            }
            return -1.0f;
        }

        private void FlattenLine(List<Vector3> verticies, Vector3 lineStart, Vector3 lineEnd, float lineWidth)
        {
            Vector3 lineNormal = lineEnd - lineStart;
            for (int i = 0; i < verticies.Count; i++)
            {
                Vector3 vertex = verticies[i];
                if (IsInLineSegment(vertex, lineStart, lineEnd))
                {
                    Vector3 closest = GetClosestPointOnLine(vertex, lineStart, lineNormal);
                    float distance = DistanceIgnoringY(closest, vertex);
                    if (distance <= lineWidth)
                    {
                        float t = distance / lineWidth;
                        t = (Mathf.Sin((t * Mathf.PI) - (Mathf.PI * 0.5f)) + 1.0f) / 2.0f;
                        vertex.y = Mathf.Lerp(closest.y, vertex.y, t);
                    }
                    verticies[i] = vertex;
                }
            }
        }

        private float DistanceIgnoringY(Vector3 p1, Vector3 p2)
        {
            p1.y = 0.0f;
            p2.y = 0.0f;
            return Vector3.Distance(p1, p2);
        }

        private bool IsInLineSegment(Vector3 position, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 toStart = lineStart - position;
            Vector3 toEnd = lineEnd - position;
            Vector3 lineNormal = lineEnd - lineStart;
            return (Vector3.Dot(toStart, lineNormal) < 0.0f && Vector3.Dot(toEnd, lineNormal) > 0.0f);
        }

        private Vector3 GetClosestPointOnLine(Vector3 position, Vector3 lineStart, Vector3 lineNormal)
        {
            position -= lineStart;
            return Vector3.Project(position, lineNormal) + lineStart;
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
