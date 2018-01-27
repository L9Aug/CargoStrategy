using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public class GraphConnection : MonoBehaviour, IGraphConnection
    {
        protected bool m_destroyed = false;
        public float m_weight;
        public GraphNode m_from;
        public GraphNode m_to;
        public float m_repairTime;

        public List<Transform> ConnectionPath = new List<Transform>();

        protected List<Units.BaseUnit> UnitsOnThisPath = new List<Units.BaseUnit>();

        public bool IsDestroyed
        {
            get
            {
                return m_destroyed;
            }

            set
            {
                m_destroyed = value;
            }
        }

        public float Weight
        {
            get
            {
                return m_weight;
            }

            set
            {
                m_weight = value;
            }
        }

        public IGraphNode From
        {
            get
            {
                return m_from;
            }
        }

        public IGraphNode To
        {
            get
            {
                return m_to;
            }
        }

        public void DestroyConnection()
        {
            if (!IsDestroyed)
            {
                // set the tile to destroyed.
                IsDestroyed = true;

                // disconnect the tile from the graph network.
                Disconnect();

                // kill all units on this path.
                for(int i = 0; i < UnitsOnThisPath.Count; ++i)
                {
                    UnitsOnThisPath[i].Kill();
                    UnitsOnThisPath.RemoveAt(i);
                    --i;
                }

                // send out an event to make units update thier paths if required.
                Units.UnitManager.Instance.CheckForPathRecalculationAfterDestruction(this);

                // begin countdown for repair.
                StartCoroutine(RepairTimer());
            }
        }

        public List<Vector3> GetRoute()
        {
            // convert transform list into vector3 list.

            List<Vector3> resultList = new List<Vector3>();

            for(int i = 0; i < ConnectionPath.Count; ++i)
            {
                resultList.Add(ConnectionPath[i].position);
            }

            return resultList;
        }

        public void Disconnect()
        {
            m_from.RemoveConnection(m_to);
            m_to.RemoveConnection(m_from);
        }

        public void Reconnect()
        {
            m_from.AddConnection(m_to);
            m_to.AddConnection(m_from);
        }

        public void RegisterUnit(Units.BaseUnit unit)
        {
            if (!UnitsOnThisPath.Contains(unit))
            {
                UnitsOnThisPath.Add(unit);
            }
        }

        public void UnRegisterUnit(Units.BaseUnit unit)
        {
            UnitsOnThisPath.Remove(unit);
        }

        protected IEnumerator RepairTimer()
        {
            float timer = 0;

            while (timer < m_repairTime)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            Reconnect();

            // send out an event to make units update thier paths if required?
        }

    }

}