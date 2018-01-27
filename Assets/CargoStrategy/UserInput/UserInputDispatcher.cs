using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Generic;

namespace CargoStrategy.UserInput
{

    public class UserInputDispatcher : Singleton<UserInputDispatcher>
    {
        public enum PlayerList
        {
            Player1,
            Player2
        }

        public event System.Action Player1FiringEvent;
        public event System.Action Player2FiringEvent;

        private void Start()
        {
            Player1FiringEvent += delegate () { Debug.Log("Player 1 firing"); };
            Player2FiringEvent += delegate () { Debug.Log("Player 2 firing"); };

        }

        private void Update()
        {
            if (Input.GetAxis("P1FireKey") > 0.5 || Input.GetAxis("P1Fire") > 0.5)
            {
                System.Action temp = Player1FiringEvent;
                if (temp != null)
                {
                    Player1FiringEvent();
                }
            }
            if (Input.GetAxis("P2FireKey") > 0.5 || Input.GetAxis("P2Fire") > 0.5)
            {
                System.Action temp = Player1FiringEvent;
                if (temp != null)
                {
                    Player2FiringEvent();
                }
            }
        }

        public float GetPlayerHorizontalMovement(PlayerList player)
        {
            float a;
            float b;
            switch (player)
            {
                case PlayerList.Player1:
                    a = Input.GetAxis("P1Horizontal");
                    b = Input.GetAxis("P1HorizontalKeyboard");
                    break;
                case PlayerList.Player2:
                    a = Input.GetAxis("P2Horizontal");
                    b = Input.GetAxis("P2HorizontalKeyboard");
                    break;
                default:
                    a = 0;
                    b = 0;
                    break;
            }

            return Mathf.Abs(a) > Mathf.Abs(b) ? a : b;
        }

        public float GetPlayerVerticalMovement(PlayerList player)
        {
            float a;
            float b;
            switch (player)
            {
                case PlayerList.Player1:
                    a = Input.GetAxis("P1Vertical");
                    b = Input.GetAxis("P1VerticalKeyboard");
                    break;
                case PlayerList.Player2:
                    a = Input.GetAxis("P2Vertical");
                    b = Input.GetAxis("P2VerticalKeyboard");
                    break;
                default:
                    a = 0;
                    b = 0;
                    break;
            }
            return Mathf.Abs(a) > Mathf.Abs(b) ? a : b;
        }

        
        public float GetPlayerHorizontalLook(PlayerList player)
        {
            switch (player)
            {
                case PlayerList.Player1:
                    return Input.GetAxis("P1LookHorizontal");
                case PlayerList.Player2:
                    return Input.GetAxis("P2LookHorizontal");
                default:
                    return -0;
            }
        }

        public float GetPlayerVerticalLook(PlayerList player)
        {
            switch (player)
            {
                case PlayerList.Player1:
                    return Input.GetAxis("P1LookVertical");
                case PlayerList.Player2:
                    return Input.GetAxis("P2LookVertical");
                default:
                    return -0;
            }
        }



    }
}