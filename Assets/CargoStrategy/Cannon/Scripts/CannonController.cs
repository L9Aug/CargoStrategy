using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.UserInput;

namespace CargoStrategy.Cannon
{
    public class CannonController : MonoBehaviour
    {
        public GameObject ProjectilePrefab;

        public GameObject cannonBase;
        public GameObject cannonGun;
        public GameObject CameraTarget;
        public GameObject CameraFocus;
        public GameObject ProjectileSpawnPoint;

        [HideInInspector]
        public ProjectileController myProjectile;

        public UserInputDispatcher.PlayerList myPlayer;

        #region constants
        private const float cannonYawSpeed = 2f;
        private const float cannonPitchSpeed = 2f;
        private const float cannonMaxPitch = 10f;
        private const float cannonMinPitch = 300f;

        private const float cannonShotPower = 300;
        #endregion



        private void Awake()
        {
            UserInputDispatcher.Instance.SubscribeToFireEvents(myPlayer, FireClicked);
        }

        private void Update()
        {
            SetCannonYaw(UserInputDispatcher.Instance.GetPlayerHorizontalMovement(myPlayer));
            SetCannonPitch(UserInputDispatcher.Instance.GetPlayerVerticalMovement(myPlayer));
        }


        #region Cannon movement
        private void SetCannonYaw(float yawAmount)
        {
            cannonBase.transform.Rotate(0, yawAmount * cannonYawSpeed, 0);
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

        #endregion

        #region Cannon Firing
        private void FireClicked()
        {
            if (myProjectile == null)
            {
                Fire();
            }
        }

        private void Fire()
        {
            myProjectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.transform.position, Quaternion.identity ).GetComponent<ProjectileController>();
            myProjectile.Fire(cannonShotPower, cannonGun.transform);
        }




        #endregion



    }
}