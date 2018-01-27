using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public interface IGraphCalculator
    {

        List<IGraphNode> Run(IGraphNode start, IGraphNode end, Units.TeamIds team);

    }

}