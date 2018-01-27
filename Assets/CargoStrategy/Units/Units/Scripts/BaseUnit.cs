using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Graphing;

namespace CargoStrategy.Units
{

    public abstract class BaseUnit : MonoBehaviour
    {
        public int m_team;
        public List<IGraphNode> Path;

        protected IGraphNode m_currentFrom;
        protected IGraphNode m_currentTo;
        protected IGraphNode m_targetNode;
        protected IGraphConnection m_currentConnection;

        List<GraphNode> m_nodeTargets;

        public void Initialise(IGraphNode startingNode, int team)
        {
            m_team = team;
            m_currentFrom = startingNode;
            GetNewPath();
        }

        private void OnDestroy()
        {
            UnitManager.Instance.UnRegisterUnit(this);
        }

        protected abstract List<GraphNode> GetNodeTargets();

        public void Kill()
        {
            if (m_currentConnection != null)
            {
                m_currentConnection.UnRegisterUnit(this);
            }
            if (UnitManager.Instance != null)
            {
                UnitManager.Instance.UnRegisterUnit(this);
            }

            // TODO death Anim?

            Destroy(this.gameObject);
        }

        public void GetNewPath()
        {
            if(m_targetNode != null)
            {
                --m_targetNode.SupplierCount;
            }

            m_nodeTargets = GetNodeTargets();

            if(m_nodeTargets == null)
            {
                Debug.LogError("No Targets to go to. Killing Self." + this);
                this.Kill();
                return;
            }

            for(int i = 0; i < m_nodeTargets.Count; ++i)
            {
                Path = GraphManager.Instance.CalculateRoute(m_currentFrom, m_nodeTargets[i], m_team);
                if(Path != null)
                {
                    m_targetNode = m_nodeTargets[i];
                    ++m_targetNode.SupplierCount;
                    break;
                }
            }

        }

        private void Update()
        {
            
        }

        protected virtual void MoveToTarget()
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
        }

    }

}