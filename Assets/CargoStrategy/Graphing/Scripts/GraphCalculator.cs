using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public class GraphCalculator : IGraphCalculator
    {

        protected List<IGraphNode> openList = new List<IGraphNode>();
        protected List<IGraphNode> closedList = new List<IGraphNode>();

        protected IGraphNode startNode;
        protected IGraphNode targetNode;
        protected int team;

        public List<IGraphNode> Run(IGraphNode start, IGraphNode end, int team)
        {
            startNode = start;
            targetNode = end;

            openList.Clear();
            closedList.Clear();

            start.CostSoFar = 0;

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
                closedList.Reverse();

                return closedList;
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

            List<IGraphNode> connections = node.GetNodeConnections();

            foreach(IGraphNode con in connections)
            {
                if (con.Team == team || con == targetNode)
                {
                    if (closedList.Contains(con))
                    {
                        if (con.EstimatedTotalCost > (node.CostSoFar + con.GetDistanceTo(node)) + (con.GetDistanceTo(targetNode)))
                        {
                            con.CostSoFar = node.CostSoFar + con.GetDistanceTo(node);
                            con.Heuristic = con.GetDistanceTo(targetNode);
                            con.EstimatedTotalCost = con.CostSoFar + con.Heuristic;
                            closedList.Remove(con);
                            openList.Add(con);
                        }
                    }
                    else
                    {
                        con.CostSoFar = node.CostSoFar + con.GetDistanceTo(node);
                        con.Heuristic = con.GetDistanceTo(targetNode);
                        con.EstimatedTotalCost = con.CostSoFar + con.Heuristic;
                        openList.Add(con);
                    }
                }
            }

        }

        protected void CloseNode(IGraphNode node)
        {
            openList.Remove(node);
            closedList.Remove(node);
        }

    }

}
