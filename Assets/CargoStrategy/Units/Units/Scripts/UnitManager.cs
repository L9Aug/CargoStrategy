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

        public void CheckForPathRecalculationAfterDestruction(IGraphConnection lostConnection)
        {
            foreach(BaseUnit unit in m_unitList)
            {
                // check to see if one end of the connection was in use for this unit.
                int fromIndex = unit.Path.IndexOf(lostConnection.From);

                if (fromIndex != -1)
                {
                    if(fromIndex > 0)
                    {
                        if (unit.Path[fromIndex - 1] == lostConnection.To)
                        {
                            // re calc path.

                        }
                    }

                    if(fromIndex < unit.Path.Count - 2)
                    {
                        if (unit.Path[fromIndex + 1] == lostConnection.To)
                        {
                            // re calc path.

                        }
                    }
                }
            }
        }

    }

}