using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public class GraphManager : Generic.Singleton<GraphManager>, IGraphManager
    {
        public List<GraphNode> NodeNetwork = new List<GraphNode>();

        public GraphManager()
        {
            
        }

        public List<IGraphNode> CalculateRoute(IGraphNode start, IGraphNode end, Units.TeamIds team)
        {
            IGraphCalculator graphingCalc = new GraphCalculator();

            return graphingCalc.Run(start, end, team);
        }

    }

}