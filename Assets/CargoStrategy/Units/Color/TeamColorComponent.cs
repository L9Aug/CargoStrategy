using CargoStrategy.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamColorComponent : MonoBehaviour {

    [SerializeField]
    private Renderer m_renderer = null;

    [SerializeField]
    private List<int> m_materialIds = new List<int>();

    private List<Material> m_materialInstances = new List<Material>();

    private Color m_team1Color = new Color32(0, 0, 254, 254);
    private Color m_team2Color = new Color32(254, 0, 0, 254);


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

    public void SetTeam(TeamIds team)
    {
        if (m_renderer != null)
        {
            for (int i = 0; i < m_materialInstances.Count; i++)
            {
                switch (team)
                {
                    case TeamIds.Player1:
                        m_materialInstances[i].SetColor("_Color", m_team1Color);
                        break;
                    case TeamIds.Player2:
                        m_materialInstances[i].SetColor("_Color", m_team2Color);
                        break;
                    default:
                        break;
                }
                
            }
        }
    }


}
