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

    }

}