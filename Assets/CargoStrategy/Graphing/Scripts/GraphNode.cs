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
        protected IGraphNode m_root;
        public int m_team;
        protected int m_supplierCount;

        public List<GraphConnection> Connections = new List<GraphConnection>();
        public List<GraphNode> NodeConnections = new List<GraphNode>();

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

        public IGraphNode Root
        {
            get
            {
                return m_root;
            }

            set
            {
                m_root = value;
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

        public int SupplierCount
        {
            get
            {
                return m_supplierCount;
            }

            set
            {
                m_supplierCount = value;
            }
        }

        public IGraphConnection GetAdjacentConnectionTo(IGraphNode to)
        {
            return Connections.Find(x => (x.To == to) || (x.From == to));
        }

        public float GetDistanceTo(IGraphNode node)
        {
            return Vector3.Distance(transform.position, node.Position);
        }

        public List<GraphNode> GetNodeConnections()
        {
            return NodeConnections;
        }

        public void AddConnection(IGraphNode node)
        {
            if (!NodeConnections.Contains((GraphNode)node))
            {
                NodeConnections.Add((GraphNode)node);
            }
        }

        public void RemoveConnection(IGraphNode node)
        {
            NodeConnections.Remove((GraphNode)node);
        }

    }

}