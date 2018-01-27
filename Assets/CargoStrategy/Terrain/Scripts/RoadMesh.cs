using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Units;

namespace CargoStrategy.Terrain
{

    public class RoadMesh : Graphing.GraphConnection {

        public static List<RoadMesh> RoadMeshes = new List<RoadMesh>();
        
        [SerializeField]
        private RoadSection m_roadPrefab;

        [SerializeField]
        private float m_prefabLength = 1.0f;

        [SerializeField]
        private float m_heightOffset = 0.0f;

        private List<RoadSection> m_roadSections = new List<RoadSection>();

        public void Awake()
        {
            RoadMeshes.Add(this);
            OnDestroyed += Destroy;
            OnRepaired += OnRepaired;
            BuildMesh();
        }

        public void OnDestroy()
        {
            RoadMeshes.Remove(this);
        }

        private void BuildMesh()
        {
            m_roadSections.Clear();
            Vector3 direction = To.Position - From.Position;
            float distance = direction.magnitude;
            direction.Normalize();
            float distanceFromstart = 0.0f;
            while (distanceFromstart < distance)
            {
                RoadSection road = Instantiate(m_roadPrefab, transform);
                road.transform.position = From.Position + (direction * distanceFromstart)+ (Vector3.up * m_heightOffset);
                road.transform.rotation = Quaternion.LookRotation(direction);
                m_roadSections.Add(road);
                distanceFromstart += m_prefabLength;
            }
        }

        private void Destroy()
        {
            for (int i = 0; i < m_roadSections.Count; i++)
            {
                m_roadSections[i].SetDestroyed(true);
            }
        }

        private void Repair()
        {
            for (int i = 0; i < m_roadSections.Count; i++)
            {
                m_roadSections[i].SetDestroyed(false);
            }
        }
    }
}
