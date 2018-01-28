using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CargoStrategy.Generic;
using CargoStrategy.Units;
using CargoStrategy.Menus;

namespace CargoStrategy.GameOver
{

    public class GameOverManager : Singleton<GameOverManager>
    {
        private List<string> m_scenesToUnload = new List<string>();

        private TeamIds m_victor;

        public TeamIds Victor
        {
            get
            {
                return m_victor;
            }
        }

        public GameObject VictoryScreenPrefab;
        private GameObject VictoryScreen;

        public void LaunchVictory(TeamIds victor)
        {
            m_victor = victor;

            VictoryScreen = Instantiate(VictoryScreenPrefab);

            Graphing.GraphManager.Instance.HaltProduction();
            
            UnitManager.Instance.KillAllUnits();

            StartCoroutine(ResetGame());
        }

        [SerializeField]
        private float GameResetDelay = 5;
        IEnumerator ResetGame()
        {
            GameObject temp = new GameObject();
            temp.transform.SetParent(transform) ;
            temp.AddComponent<UnityEngine.Camera>();
            RemoveOtherScenes();

            yield return new WaitForSeconds(GameResetDelay);


            SceneStreamingManager.Instance.OnAllScenesLoaded += SceneLoaded;
            SceneStreamingManager.Instance.LoadScene(SceneStreamingSettings.IntroScene);

            Destroy(temp);

        }
        private void RemoveOtherScenes()
        {
            for (int i = 0; i < SceneStreamingSettings.GameSceneSuffixes.Length; i++)
            {
                m_scenesToUnload.Add(SceneStreamingSettings.GameScenePrefix + SceneStreamingSettings.GameSceneSuffixes[i]);
            }

            SceneStreamingManager.Instance.UnloadScenes(m_scenesToUnload);
        }

        private void SceneLoaded()
        {
            SceneStreamingManager.Instance.OnAllScenesActive += ResetGameOverManager;
            SceneStreamingManager.Instance.ActivateScenes();
        }


        private void ResetGameOverManager()
        {
            SceneStreamingManager.Instance.OnAllScenesActive -= RemoveOtherScenes;
            Destroy(VictoryScreen);
        }

    }

}