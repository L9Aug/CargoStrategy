using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Units
{

    public class HomeBase : BaseBuilding
    {

        public override void Convert(TeamIds nTeam)
        {
            base.Convert(nTeam);

            // TODO [x] Victory for nTeam.
            GameOver.GameOverManager.Instance.LaunchVictory(nTeam);
        }

    }

}