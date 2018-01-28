using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Units
{

    public class Farm : BaseBuilding
    {

        protected override float GetProductionModifierFromStorage()
        {
            return 1;
        }

        protected override void Update()
        {
            if (ProductionOutput != null && m_team != TeamIds.Neutral)
            {
                // increase production progress.
                m_productionProgress += OptimalOutputPerSecond * Time.deltaTime * GetProductionModifierFromStorage();

                if (m_productionProgress > 1)
                {
                    --m_productionProgress;
                    CreateUnit();
                }
            }
        }

    }

}