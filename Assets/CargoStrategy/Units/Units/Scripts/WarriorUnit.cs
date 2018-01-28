using System;
using System.Collections;
using System.Collections.Generic;
using CargoStrategy.Graphing;
using UnityEngine;

namespace CargoStrategy.Units
{

    public class WarriorUnit : BaseUnit
    {
        public GameObject KillEffect;

        protected override List<GraphNode> GetNodeTargets()
        {
            List<GraphNode> resultList = GraphManager.Instance.NodeNetwork.FindAll(x => x.Team != m_team);

            if (resultList == null || resultList.Count == 0) return null;

            resultList.Sort((a, b) => { return a.SupplierCount[((int)m_team) - 1] < b.SupplierCount[((int)m_team) - 1] ? -1 : 1; });

            return resultList;
        }

        protected override void ArrivedAtTarget()
        {
            //Debug.Log("Warrior Arrived");

            --m_targetNode.SupplierCount[((int)m_team) - 1];

            ((BaseBuilding)m_targetNode).Convert(m_team);

            base.ArrivedAtTarget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<BaseUnit>() != null)
            {
                if (other.GetComponent<BaseUnit>().m_team != m_team)
                {
                    if (KillEffect != null)
                    {
                        GameObject nKillEffect = Instantiate(KillEffect);
                        nKillEffect.transform.position = other.transform.position;
                    }
                    other.GetComponent<BaseUnit>().Kill();
                }
            }
        }

    }

}