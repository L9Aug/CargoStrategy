using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Graphing;

namespace CargoStrategy.Units
{

    public enum TeamIds { Neutral = 0, Player1 = 1, Player2 = 2 }

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

        [SerializeField]
        private TeamColorComponent m_colorComponent = null;

        public void HaltProduction()
        {
            storedSupply = 0;
            InputTarget = 100;
            OptimalOutputPerSecond = 0;
            m_productionProgress = 0;
        }

        private void Awake()
        {
            GraphManager.Instance.NodeNetwork.Add(this);
            // TODO SetColour?
            if (m_colorComponent != null)
            {
                m_colorComponent.SetTeam(m_team);
            }
        }

        private void OnDestroy()
        {
            if (GraphManager.HasInstance())
            {
                GraphManager.Instance.NodeNetwork.Remove(this);
            }
        }

        private void Update()
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

        protected virtual float GetProductionModifierFromStorage()
        {
            return Mathf.Clamp01(((float)storedSupply / (float)InputTarget));
        }

        protected void CreateUnit()
        {
            // create the output unit.
            BaseUnit nUnit = Instantiate(ProductionOutput);

            nUnit.transform.position = transform.position;
            nUnit.transform.rotation = transform.rotation;

            // register the unit to the manager.
            UnitManager.Instance.RegisterNewUnit(nUnit);

            // initalise the unit.
            nUnit.Initialise(this, m_team);
        }

        public void StockArrived()
        {
            ++storedSupply;
        }

        public virtual void Convert(TeamIds nTeam)
        {
            m_team = nTeam;
            storedSupply = 0;
            m_productionProgress = 0;
            UnitManager.Instance.BuildingConverted(this);

            // TODO Change Colour?
            if (m_colorComponent != null)
            {
                m_colorComponent.SetTeam(m_team);
            }
        }

    }

}