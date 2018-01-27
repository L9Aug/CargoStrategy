using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSection : MonoBehaviour {

    [SerializeField]
    private GameObject m_destroyedGameObject = null;

    [SerializeField]
    private GameObject m_activeGameObject = null;


    public void SetDestroyed(bool destroyed)
    {
        m_destroyedGameObject.SetActive(destroyed);
        m_activeGameObject.SetActive(!destroyed);
    }

}
