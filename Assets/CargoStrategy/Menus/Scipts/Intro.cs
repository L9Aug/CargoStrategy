using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Menus
{

    public class Intro : MonoBehaviour {

        private List<string> m_scenesToLoad = new List<string>();

        // Use this for initialization
        IEnumerator Start ()
        {
            if (!SceneStreamingManager.HasInstance())
            {
                SceneStreamingManager.LoadManagers();
                yield return new WaitForEndOfFrame();
            }
            LoadMainScene();
        }

        private void LoadMainScene()
        {
            for (int i = 0; i < SceneStreamingSettings.GameSceneSuffixes.Length; i++)
            {
                m_scenesToLoad.Add(SceneStreamingSettings.GameScenePrefix + SceneStreamingSettings.GameSceneSuffixes[i]);
            }
            SceneStreamingManager.Instance.OnAllScenesLoaded += CompleteLoad;
            SceneStreamingManager.Instance.LoadScenes(m_scenesToLoad);
        }

        private void CompleteLoad()
        {
            SceneStreamingManager.Instance.OnAllScenesLoaded -= CompleteLoad;
            SceneStreamingManager.Instance.OnAllScenesActive += UnloadIntro;
            SceneStreamingManager.Instance.ActivateScenes();
        }

        private void UnloadIntro()
        {
            SceneStreamingManager.Instance.OnAllScenesActive -= UnloadIntro;
            SceneStreamingManager.Instance.UnloadScene(gameObject.scene.name);
            DynamicGI.UpdateEnvironment();
        }
    }

}
