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

        public List<IGraphNode> CalculateRoute(IGraphNode start, IGraphNode end, Units.TeamIds team)
        {
            IGraphCalculator graphingCalc = new GraphCalculator();

            return graphingCalc.Run(start, end, team);
        }

        public void HaltProduction()
        {
            for(int i = 0; i < m_nodeNetwork.Count; ++i)
            {
                ((Units.BaseBuilding)m_nodeNetwork[i]).HaltProduction();
            }
        }

    }

}