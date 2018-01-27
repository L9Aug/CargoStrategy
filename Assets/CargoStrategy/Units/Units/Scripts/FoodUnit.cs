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

            resultList.Sort((a, b) => { return a.SupplierCount < b.SupplierCount ? -1 : 1; });

            //Debug.Log(resultList[0].SupplierCount + " " + resultList[resultList.Count - 1].SupplierCount + " list size: " + resultList.Count);

            return resultList;
        }

        protected override void ArrivedAtTarget()
        {
            --m_targetNode.SupplierCount;

            ((BaseBuilding)m_targetNode).StockArrived();

            base.ArrivedAtTarget();
        }

    }

}