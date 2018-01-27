using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Graphing
{

    public interface IGraphConnection
    {

        bool IsDestroyed
        {
            get;
            set;
        }

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

        List<Vector3> GetRoute();

        void Reconnect();
        void Disconnect();

        void DestroyConnection();

        void RegisterUnit();
        void UnRegisterUnit();

    }

}