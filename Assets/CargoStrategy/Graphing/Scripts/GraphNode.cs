using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public class GraphNode : MonoBehaviour, IGraphNode
    {

        protected float m_costSoFar;
        protected float m_estimatedTotalCost;
        protected float m_heuristic;
        protected int m_team;

        public List<IGraphConnection> Connections = new List<IGraphConnection>();
        public List<IGraphNode> NodeConnections = new List<IGraphNode>();

        public float CostSoFar
        {
            get
            {
                return m_costSoFar;
            }

            set
            {
                m_costSoFar = value;
            }
        }

        public float EstimatedTotalCost
        {
            get
            {
                return m_estimatedTotalCost;
            }

            set
            {
                m_estimatedTotalCost = value;
            }
        }

        public float Heuristic
        {
            get
            {
                return m_heuristic;
            }
            set
            {
                m_heuristic = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
        }

        public int Team
        {
            get
            {
                return m_team;
            }
        }

        public IGraphConnection GetAdjacentConnectionTo(IGraphNode to)
        {
            return Connections.Find(x => x.To == to);
        }

        public float GetDistanceTo(IGraphNode node)
        {
            return Vector3.Distance(transform.position, node.Position);
        }

        public List<IGraphNode> GetNodeConnections()
        {
            return NodeConnections;
        }

    }

}