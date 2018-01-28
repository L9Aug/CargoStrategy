using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Graphing;

namespace CargoStrategy.Units
{

    public class UnitManager : Generic.Singleton<UnitManager>
    {

        protected List<BaseUnit> m_unitList = new List<BaseUnit>();

        public void RegisterNewUnit(BaseUnit unit)
        {
            if (!m_unitList.Contains(unit))
            {
                m_unitList.Add(unit);
                unit.transform.SetParent(transform, true);
            }
        }

        public void UnRegisterUnit(BaseUnit unit)
        {
            m_unitList.Remove(unit);
        }

        public void KillAllUnits()
        {
            for(int i = 0; i < m_unitList.Count; ++i)
            {
                if (m_unitList[i] != null)
                {
                    m_unitList[i].Kill();
                }
                else
                {
                    m_unitList.RemoveAt(i);
                }
                --i;
            }
        }

        public void BuildingConverted(IGraphNode node)
        {
            // if a building that the unit used in it's future path became converted then look for a new strategy.
            /*
            foreach (BaseUnit unit in m_unitList)
            {
                int nodeIndex = unit.Path.IndexOf(node);

                if(nodeIndex > 0)
                {
                    unit.GetNewPath();
                }
            }
            */

            for (int i = 0; i < m_unitList.Count; ++i)
            {
                if (m_unitList[i] == null)
                {
                    m_unitList.RemoveAt(i);
                    --i;
                }
                else
                {
                    if (m_unitList[i].Path != null)
                    {
                        int nodeIndex = m_unitList[i].Path.IndexOf(node);

                        if (nodeIndex != -1)
                        {
                            m_unitList[i].GetNewPath();
                        }
                    }
                }
            }
        }

        public void CheckForPathRecalculationAfterDestruction(IGraphConnection lostConnection)
        {
            for (int i = 0; i < m_unitList.Count; ++i)
            {
                if (m_unitList[i] == null)
                {
                    m_unitList.RemoveAt(i);
                    --i;
                }
                else
                {
                    if (m_unitList[i].Path != null)
                    {
                        int fromIndex = m_unitList[i].Path.IndexOf(lostConnection.From);

                        if (fromIndex != -1)
                        {
                            m_unitList[i].GetNewPath();
                        }
                        else
                        {
                            fromIndex = m_unitList[i].Path.IndexOf(lostConnection.To);

                            if(fromIndex != -1)
                            {
                                m_unitList[i].GetNewPath();
                            }

                        }
                    }
                }
            }

            /*
            foreach (BaseUnit unit in m_unitList)
            {
                // check to see if one end of the connection was in use for this unit.
                int fromIndex = unit.Path.IndexOf(lostConnection.From);

                if (fromIndex != -1)
                {
                    if (fromIndex > 0)
                    {
                        if (unit.Path[fromIndex - 1] == lostConnection.To)
                        {
                            // re calc path.
                            unit.GetNewPath();
                        }
                    }

                    if (fromIndex < unit.Path.Count - 2)
                    {
                        if (unit.Path[fromIndex + 1] == lostConnection.To)
                        {
                            // re calc path.
                            unit.GetNewPath();
                        }
                    }
                }
            }
            */
        }

    }

}