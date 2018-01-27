using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public interface IGraphManager
    {

        IGraphRoute CalculateRoute(IGraphNode start, IGraphNode end, int team);

    }

}