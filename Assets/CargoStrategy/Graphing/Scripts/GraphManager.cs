using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Terrain;

namespace CargoStrategy.Graphing
{

    public class GraphManager : Generic.Singleton<GraphManager>, IGraphManager
    {
        [SerializeField]
        private RoadMesh m_roadPrefab;

        private List<GraphNode> m_nodeNetwork = new List<GraphNode>();

        IGraphCalculator m_graphCalculator;

        public GraphManager()
        {
            
        }

        public List<GraphNode> NodeNetwork
        {
            get
            {
                return m_nodeNetwork;
            }

            set
            {
                m_nodeNetwork = value;
            }
        }

        public RoadMesh RoadPrefab
        {
            get
            {
                return m_roadPrefab;
            }
        }

        public List<IGraphNode> CalculateRoute(IGraphNode start, IGraphNode end, int team)
        {
            IGraphCalculator graphingCalc = new GraphCalculator();

            return graphingCalc.Run(start, end, team);
        }

    }

}