using System;
using System.Collections;
using System.Collections.Generic;
using CargoStrategy.Graphing;
using UnityEngine;

namespace CargoStrategy.Units
{

    public class WarriorUnit : BaseUnit
    {
        protected override List<GraphNode> GetNodeTargets()
        {
            List<GraphNode> resultList = GraphManager.Instance.NodeNetwork.FindAll(x => x.Team != m_team);

            if (resultList == null || resultList.Count == 0) return null;

            resultList.Sort((a, b) => {
                return Vector3.Distance(this.transform.position, a.transform.position).CompareTo(
                                    Vector3.Distance(this.transform.position, b.transform.position));
            });

            //Debug.Log(resultList[0].SupplierCount + " " + resultList[resultList.Count - 1].SupplierCount + " list size: " + resultList.Count);

            return resultList;
        }

        protected override void ArrivedAtTarget()
        {
            Debug.Log("Warrior Arrived");

            ((BaseBuilding)m_targetNode).Convert(m_team);

            base.ArrivedAtTarget();
        }

    }

}