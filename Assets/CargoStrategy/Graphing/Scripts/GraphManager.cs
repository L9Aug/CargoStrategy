using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public class GraphManager : IGraphManager
    {
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