using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Terrain
{

    public class RoadMesh : MonoBehaviour {

        [SerializeField]
        private Transform m_start;

        [SerializeField]
        private Transform m_end;

        [SerializeField]
        private RoadSection m_roadPrefab;

        [SerializeField]
        private float m_prefabLength;

        [SerializeField]
        private float m_heightOffset = 0.0f;

        public delegate void OnDestroyedDelegate();
        public OnDestroyedDelegate OnDestroyed;

        private List<RoadSection> m_roadSections = new List<RoadSection>();

        public Transform Start
        {
            get
            {
                return m_start;
            }
        }

        public Transform End
        {
            get
            {
                return m_end;
            }
        }

        public void Awake()
        {
            RebuildMesh();
        }

        private void RebuildMesh()
        {
            m_roadSections.Clear();
            Vector3 direction = m_end.position - m_start.position;
            float distance = direction.magnitude;
            direction.Normalize();
            float distanceFromstart = 0.0f;
            while (distanceFromstart < distance)
            {
                RoadSection road = Instantiate(m_roadPrefab, transform);
                road.transform.position = m_start.position + (direction * distanceFromstart)+ (Vector3.up * m_heightOffset);
                road.transform.LookAt(m_end, Vector3.up);
                m_roadSections.Add(road);
                distanceFromstart += m_prefabLength;
            }
        }

        public void SetDestroy(bool destroyed)
        {
            for (int i =0;i < m_roadSections.Count; i++)
            {
                m_roadSections[i].SetDestroyed(destroyed);
            }
            if (destroyed && OnDestroyed != null)
            {
                OnDestroyed();
            }
        }

    }
}
