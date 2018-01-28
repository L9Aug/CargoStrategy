using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Menus
{

    public class Intro : MonoBehaviour {

        private List<string> m_scenesToLoad = new List<string>();

        [SerializeField]
        private List<GameObject> TurnMeOnList;


        // Use this for initialization
        IEnumerator Start ()
        {
            if (!SceneStreamingManager.HasInstance())
            {
                SceneStreamingManager.LoadManagers();
                yield return new WaitForEndOfFrame();
            }

            foreach (GameObject go in TurnMeOnList)
            {
                go.SetActive(true);
            }

            bool Player1Ready = false;
            bool Player2Ready = false;
            UserInput.UserInputDispatcher.Instance.Player1Start += delegate () { Player1Ready = !Player1Ready; };
            UserInput.UserInputDispatcher.Instance.Player2Start += delegate () { Player2Ready = !Player2Ready; };


            while (!Player1Ready || !Player2Ready)
            {
                yield return null;
            }

            UserInput.UserInputDispatcher.Instance.ResetEvents();
            
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
            SceneStreamingManager.Instance.UnloadScene(gameObject.scene.name);
            SceneStreamingManager.Instance.ActivateScenes();
            


        }

        private void UnloadIntro()
        {
        }
    }

}
