using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public class GraphManager : Generic.Singleton<GraphManager>, IGraphManager
    {
        public List<GraphNode> NodeNetwork = new List<GraphNode>();

        IGraphCalculator m_graphCalculator;

        public GraphManager()
        {
            m_graphCalculator = new GraphCalculator();
        }

        public List<IGraphNode> CalculateRoute(IGraphNode start, IGraphNode end, int team)
        {
            return m_graphCalculator.Run(start, end, team);
        }

    }

}