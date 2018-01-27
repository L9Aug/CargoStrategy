using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using CargoStrategy.Generic;

namespace CargoStrategy.Menus
{

    public class SceneStreamingManager : Singleton<SceneStreamingManager>
    {
        private List<AsyncOperation> m_sceneOperations = new List<AsyncOperation>();

        public delegate void OnScenesEventDelegate();

        public OnScenesEventDelegate OnAllScenesLoaded;
        private bool m_loadingScenes = false;

        public OnScenesEventDelegate OnAllScenesActive;
        private bool m_activatingScenes = false;

        public static void LoadManagers()
        {
            SceneManager.LoadScene(SceneStreamingSettings.ManagersScene, LoadSceneMode.Additive);
        }

        public void LoadScenes(List<string> scenes)
        {
            m_loadingScenes = true;
            for (int i = 0; i < scenes.Count; i++)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(scenes[i], LoadSceneMode.Additive);
                operation.allowSceneActivation = false;
                m_sceneOperations.Add(operation);
            }
        }

        public void LoadScene(string scene)
        {
            m_loadingScenes = true;
            AsyncOperation operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            operation.allowSceneActivation = false;
            m_sceneOperations.Add(operation);
        }

        public void UnloadScenes(List<string> scenes)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                SceneManager.UnloadSceneAsync(scenes[i]);
            }
        }

        public void UnloadScene(string scene)
        {
            SceneManager.UnloadSceneAsync(scene);
        }


        public void ActivateScenes()
        {
            m_activatingScenes = true;
            for (int i = 0; i < m_sceneOperations.Count; i++)
            {
                m_sceneOperations[i].allowSceneActivation = true;
            }
        }


        private void Update()
        {
            if (m_loadingScenes)
            {
                bool finished = true;
                for (int i = 0; i< m_sceneOperations.Count; i++)
                {
                    if (m_sceneOperations[i].progress < 0.9f)
                    {
                        finished = false;
                    }
                }
                if (finished)
                {
                    m_loadingScenes = false;
                    if (OnAllScenesLoaded != null)
                    {
                        OnAllScenesLoaded();
                    }
                } 
            }
            if (m_activatingScenes)
            {
                bool finished = true;
                for (int i = 0; i < m_sceneOperations.Count; i++)
                {
                    if (!m_sceneOperations[i].isDone)
                    {
                        finished = false;
                    }
                }
                if (finished)
                {
                    m_activatingScenes = false;
                    if (OnAllScenesActive != null)
                    {
                        OnAllScenesActive();
                    }
                    m_sceneOperations.Clear();
                }
            }
        }

    }


}

