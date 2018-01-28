using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorComponent : MonoBehaviour {

    [SerializeField]
    private Renderer m_renderer = null;

    [SerializeField]
    private List<int> m_materialIds = new List<int>();

    private List<Material> m_materialInstances = new List<Material>();

    private void SetTeam(int team)
    {

    }


}
