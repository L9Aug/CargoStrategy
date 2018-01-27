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
        
        List<IGraphNode> GetNodeConnections();

        IGraphConnection GetAdjacentConnectionTo(IGraphNode to);

        float GetDistanceTo(IGraphNode node);

    }

}