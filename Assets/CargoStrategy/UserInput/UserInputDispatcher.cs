using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Generic;

namespace CargoStrategy.UserInput
{

    public class UserInputDispatcher : Singleton<UserInputDispatcher>
    {
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

        public float GetPlayer1HorizontalMovement()
        {
            return Input.GetAxis("P1Horizontal");
        }
        public float GetPlayer1VerticalMovement()
        {
            return Input.GetAxis("P2Vertical");
        }

        public float GetPlayer1HorizontalLook()
        {
            return Input.GetAxis("P1LookHorizontal");
        }
        public float GetPlayer1VerticalLook()
        {
            return Input.GetAxis("P1LookVertical");
        }

        public float GetPlayer2HorizontalMovement()
        {
            return Input.GetAxis("P2Horizontal");
        }
        public float GetPlayer2VerticalMovement()
        {
            return Input.GetAxis("P2Vertical");
        }

        public float GetPlayer2HorizontalLook()
        {
            return Input.GetAxis("P2LookHorizontal");
        }
        public float GetPlayer2VerticalLook()
        {
            return Input.GetAxis("P2LookVertical");
        }






    }
}