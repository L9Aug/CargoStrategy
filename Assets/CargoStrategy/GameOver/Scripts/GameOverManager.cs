using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Generic;
using CargoStrategy.Units;

namespace CargoStrategy.GameOver
{

    public class GameOverManager : Singleton<GameOverManager>
    {

        private TeamIds m_victor;

        public TeamIds Victor
        {
            get
            {
                return m_victor;
            }
        }

        public GameObject VictoryScreen;

        public void LaunchVictory(TeamIds victor)
        {
            m_victor = victor;

            Instantiate(VictoryScreen);

            Graphing.GraphManager.Instance.HaltProduction();
            
            UnitManager.Instance.KillAllUnits();

        }

    }

}