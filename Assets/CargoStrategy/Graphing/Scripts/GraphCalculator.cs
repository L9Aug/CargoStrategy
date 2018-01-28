using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public class GraphCalculator : IGraphCalculator
    {

        protected List<IGraphNode> openList = new List<IGraphNode>();
        protected List<IGraphNode> closedList = new List<IGraphNode>();
        protected List<IGraphNode> path = new List<IGraphNode>();

        protected List<IGraphNode> NodeMap;

        protected IGraphNode startNode;
        protected IGraphNode targetNode;
        protected Units.TeamIds m_team;

        public List<IGraphNode> Run(IGraphNode start, IGraphNode end, Units.TeamIds team)
        {
            startNode = start;
            targetNode = end;
            m_team = team;

            openList.Clear();
            closedList.Clear();
            path.Clear();

            start.CostSoFar = 0;
            start.Root = null;

            openList.Add(start);

            bool targetReached = false;

            while(openList.Count > 0 && !targetReached)
            {
                IGraphNode current = GetLowestEstimatedTotalCost();

                if (current == null) break;

                if (current == targetNode) targetReached = true;

                AddConnectionsToOpen(current);
            }

            if (targetReached)
            {                
                IGraphNode current = targetNode;
                
                while(current.Root != null)
                {
                    path.Add(current);
                    current = current.Root;
                    path[path.Count - 1].Root = null;
                }

                path.Add(current);
                path.Reverse();

                return path;
            }
            else
            {
                return null;
            }
        }

        protected IGraphNode GetLowestEstimatedTotalCost()
        {
            IGraphNode lowest = openList[0];

            for(int i = 1; i < openList.Count; ++i)
            {
                if(openList[i].EstimatedTotalCost < lowest.EstimatedTotalCost)
                {
                    lowest = openList[i];
                }
            }

            return lowest;
        }

        protected void AddConnectionsToOpen(IGraphNode node)
        {
            // close the current node.
            CloseNode(node);

            List<GraphNode> connections = node.GetNodeConnections();

            foreach(IGraphNode con in connections)
            {
                if (con.Team == m_team || con == targetNode)
                {
                    if (closedList.Contains(con))
                    {
                        if (con.EstimatedTotalCost > (node.CostSoFar + con.GetDistanceTo(node)) + (con.GetDistanceTo(targetNode)))
                        {
                            con.CostSoFar = node.CostSoFar + con.GetDistanceTo(node);
                            con.Heuristic = con.GetDistanceTo(targetNode);
                            con.EstimatedTotalCost = con.CostSoFar + con.Heuristic;
                            con.Root = node;
                            closedList.Remove(con);
                            openList.Add(con);
                        }
                    }
                    else
                    {
                        con.CostSoFar = node.CostSoFar + con.GetDistanceTo(node);
                        con.Heuristic = con.GetDistanceTo(targetNode);
                        con.EstimatedTotalCost = con.CostSoFar + con.Heuristic;
                        con.Root = node;
                        openList.Add(con);
                    }
                }
            }

        }

        protected void CloseNode(IGraphNode node)
        {
            openList.Remove(node);
            closedList.Add(node);
        }

    }

}
