using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.UserInput;

namespace CargoStrategy.Cannon
{
    public class CannonController : MonoBehaviour
    {
        public GameObject cannonBase;
        public GameObject cannonGun;

        public UserInputDispatcher.PlayerList myPlayer;

        #region constants
        private const float cannonYawSpeed = 2f;
        private const float cannonPitchSpeed = 2f;
        private const float cannonMaxPitch = 10f;
        private const float cannonMinPitch = 300f;
        #endregion


        private void Update()
        {
            SetCannonYaw(UserInputDispatcher.Instance.GetPlayerHorizontalMovement(myPlayer));
            SetCannonPitch(UserInputDispatcher.Instance.GetPlayerVerticalMovement(myPlayer));
        }

        private void SetCannonYaw(float yawAmount)
        {
            cannonBase.transform.Rotate(0, yawAmount * cannonYawSpeed, 0);
        }
        
        private float Mod(float a, float b)
        {
            return (a % b < 0 ? a + b : a);
        }

        private void SetCannonPitch(float pitchAmount)
        {
            float rot = cannonGun.transform.localRotation.eulerAngles.x + pitchAmount * cannonPitchSpeed;

            if (rot < 180)
            {
                rot = rot < cannonMaxPitch ? rot : cannonMaxPitch;
            }
            else
            {
                rot = rot > cannonMinPitch ? rot : cannonMinPitch;
            }

        
            cannonGun.transform.localRotation = Quaternion.Euler(rot, 0 , 0);
        }

       
    }
}