using System;
using System.Collections;
using System.Collections.Generic;
using CargoStrategy.Graphing;
using UnityEngine;
using CargoStrategy.Graphing;

namespace CargoStrategy.Units
{

    public class FoodUnit : BaseUnit
    {
        protected override List<GraphNode> GetNodeTargets()
        {
            List<GraphNode> resultList = GraphManager.Instance.NodeNetwork.FindAll(x => x.Team == m_team && x is City);

            if (resultList == null || resultList.Count == 0) return null;

            resultList.Sort((a, b) => { return a.SupplierCount[((int)m_team) - 1] < b.SupplierCount[((int)m_team) - 1] ? -1 : 1; });

            return resultList;
        }

        protected override void ArrivedAtTarget()
        {
            --m_targetNode.SupplierCount[((int)m_team) - 1];

            if (((BaseBuilding)m_targetNode).ProductionInput.GetType() == this.GetType())
            {
                ((BaseBuilding)m_targetNode).StockArrived();
            }

            base.ArrivedAtTarget();
        }

    }

}