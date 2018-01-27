using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Graphing;

namespace CargoStrategy.Units
{

    public abstract class BaseBuilding : GraphNode 
    {
        // The Unit that this building creates.
        public BaseUnit ProductionOutput;

        // the amount of a unit that is produced per second with optimal input.
        public float OptimalOutputPerSecond;

        // the unit that this building requires to run production.
        public BaseUnit ProductionInput;

        // the amount of the required unit to run at optimal speed.
        public int InputTarget;

        protected int storedSupply;

        // the current progress towards creating a new unit.
        protected float m_productionProgress = 0;

        private void Update()
        {
            if (ProductionOutput != null)
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

        protected virtual float GetProductionModifierFromStorage()
        {
            return ((float)storedSupply / (float)InputTarget);
        }

        protected void CreateUnit()
        {
            // create the output unit.
            BaseUnit nUnit = Instantiate(ProductionOutput);

            nUnit.transform.position = transform.position;

            // register the unit to the manager.
            UnitManager.Instance.RegisterNewUnit(nUnit);

            // initalise the unit.
            nUnit.Initialise(this, m_team);
        }

    }

}