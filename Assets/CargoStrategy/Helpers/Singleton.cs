using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Generic
{

    public class Singleton<T> : MonoBehaviour
        where T : Singleton<T>
    {
        private static T m_instance;

        protected virtual void Awake()
        {
            if (Instance != this)
            {
                Debug.Log("Multiple singltons of type" + typeof(T).Name + ", destroying duplicate.");
                Destroy(gameObject);
            }
        }

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                    if (m_instance == null)
                    {
                        GameObject obj = new GameObject();
                        m_instance = obj.AddComponent<T>();
                    }
                }
                return m_instance;
            }
        }

        public static bool HasInstance()
        {
            return m_instance != null;
        }
    }

}
