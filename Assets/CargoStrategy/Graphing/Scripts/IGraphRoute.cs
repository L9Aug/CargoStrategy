using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public interface IGraphRoute 
    {

        List<IGraphRoute> GetFullRoute();

        IGraphConnection GetNextNode();

        IGraphRoute CreateRoute(List<IGraphNode> closedList);

    }

}