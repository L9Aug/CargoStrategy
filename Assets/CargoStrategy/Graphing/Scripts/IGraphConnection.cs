using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public interface IGraphConnection
    {

        float Weight
        {
            get;
            set;
        }

        IGraphNode From
        {
            get;
        }

        IGraphNode To
        {
            get;
        }

    }

}