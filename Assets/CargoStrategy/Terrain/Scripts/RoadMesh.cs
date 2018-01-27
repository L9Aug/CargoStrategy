using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Terrain
{

    public class RoadMesh : MonoBehaviour {

        [SerializeField]
        private Vector3 start;

        [SerializeField]
        private Vector3 end;

        [SerializeField]
        private TerrainMesh m_mesh;
        
        public void Awake()
        {
            
        }


        public float GetDistanceToLineSegment(Vector3 position, Vector3 lineStart, Vector3 lineEnd)
        {

        }
    }
}
