using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorComponent : MonoBehaviour {

    [SerializeField]
    private Renderer m_renderer = null;

    [SerializeField]
    private List<int> m_materialIds = new List<int>();

    private List<Material> m_materialInstances = new List<Material>();

    private Color m_team1Color = new Color32(80, 93, 134, 254);
    private Color m_team2Color = new Color32(142, 76, 67, 254);


    private void Awake()
    {
        if (m_renderer != null)
        {
            for (int i =0; i < m_materialIds.Count; i++)
            {
                if (m_materialIds[i] < m_renderer.materials.Length)
                {
                    m_materialInstances.Add(m_renderer.materials[m_materialIds[i]]);
                }
            }
        }
    }

    private void SetTeam(int team)
    {
        if (m_renderer != null)
        {
            for (int i = 0; i < m_materialInstances.Count; i++)
            {
                if (team == 1)
                {
                    m_materialInstances[i].SetColor("_Color", m_team1Color);
                }
                if (team == 2)
                {
                    m_materialInstances[i].SetColor("_Color", m_team2Color);
                }
            }
        }
    }


}
