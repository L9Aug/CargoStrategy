using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public interface IGraphNode
    {

        float CostSoFar
        {
            get;
            set;
        }

        float EstimatedTotalCost
        {
            get;
            set;
        }

        float Heuristic
        {
            get;
            set;
        }

        IGraphNode Root
        {
            get;
            set;
        }

        Vector3 Position
        {
            get;
        }

        Units.TeamIds Team
        {
            get;
        }

        int SupplierCount
        {
            get;
            set;
        }
        
        List<GraphNode> GetNodeConnections();

        IGraphConnection GetAdjacentConnectionTo(IGraphNode to);

        float GetDistanceTo(IGraphNode node);

        void AddConnection(IGraphNode node);

        void RemoveConnection(IGraphNode node);


    }

}