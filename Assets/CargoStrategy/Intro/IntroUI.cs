using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CargoStrategy.Intro {
    public class IntroUI : MonoBehaviour
    {
        public CargoStrategy.Cannon.CannonController myCannon;

        public RawImage ControlImage;
        public RawImage mainDisplay;
        public RawImage ReadyImage;


        // Use this for initialization
        void Start() {

            switch (myCannon.myPlayer)
            {
                case UserInput.UserInputDispatcher.PlayerList.Player1:
                    UserInput.UserInputDispatcher.Instance.Player1Start += ToggleReadyStatus;
                    break;
                case UserInput.UserInputDispatcher.PlayerList.Player2:
                    UserInput.UserInputDispatcher.Instance.Player2Start += ToggleReadyStatus;
                    break;
                default:
                    break;
            }
        }

        void ToggleReadyStatus()
        {
            mainDisplay.gameObject.SetActive(!mainDisplay.gameObject.activeSelf);
            ReadyImage.gameObject.SetActive(!ReadyImage.gameObject.activeSelf);
        }

        // Update is called once per frame
        void Update()
        {
            if (UserInput.UserInputDispatcher.Instance.GetPlayerSelect(myCannon.myPlayer))
            {
                ControlImage.gameObject.SetActive(true);
            }
            else
            {
                ControlImage.gameObject.SetActive(false);
            }


        }
    }
}