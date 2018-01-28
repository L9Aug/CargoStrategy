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
        public GameObject cannonBarrelMovable;
        public GameObject CameraTarget;
        public GameObject CameraFocus;
        public GameObject ProjectileSpawnPoint;


        public List<ParticleSystem> FireParticleEffects = new List<ParticleSystem> ();

        [HideInInspector]
        public ProjectileController myProjectile;

        public UserInputDispatcher.PlayerList myPlayer;

        #region constants
        private const float cannonYawSpeed = 2f;
        private const float cannonPitchSpeed = 2f;
        private const float cannonMaxPitch = 100;
        private const float cannonMinPitch = 30;

        private const float cannonShotPower = 100;

        private const float defaultPitch = 90;
        #endregion



        private void Awake()
        {
            UserInputDispatcher.Instance.SubscribeToFireEvents(myPlayer, FireClicked);
        }

        private void Update()
        {
            if (myProjectile == null)
            {
                SetCannonYaw(UserInputDispatcher.Instance.GetPlayerHorizontalMovement(myPlayer));
                SetCannonPitch(UserInputDispatcher.Instance.GetPlayerVerticalMovement(myPlayer));
            }
        }


        #region Cannon movement
        private void SetCannonYaw(float yawAmount)
        {
            cannonBase.transform.Rotate(0, yawAmount * cannonYawSpeed, 0, Space.World);
        }

        float pitch = defaultPitch;

        private void SetCannonPitch(float pitchAmount)
        {
            //float rot = 100;//cannonGun.transform.localRotation.eulerAngles.x + pitchAmount * cannonPitchSpeed;//, cannonMinPitch, cannonMaxPitch);
            pitch = Mathf.Clamp(pitch + pitchAmount * cannonPitchSpeed, cannonMinPitch, cannonMaxPitch);

            cannonGun.transform.localRotation = Quaternion.Euler(pitch, 0 , 0);
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

            RecoilTime = 0;
            StartCoroutine(Recoil());

            foreach (ParticleSystem p in FireParticleEffects)
            {
                p.Stop();
                p.Play();
            }
        }

        
        float RecoilDuration = 0.25f;
        float RecoilTime;
        float RecoilDistance = 1.2f;
        IEnumerator Recoil()
        {
            float startZ = cannonBarrelMovable.transform.localPosition.z;
            while (RecoilTime <= RecoilDuration)
            {
                Mathf.Clamp(RecoilTime += Time.deltaTime, 0, RecoilDuration);
                Debug.Log(RecoilDistance * (Mathf.Pow((2 * (RecoilTime / RecoilDuration - 0.5f)), 2) - 1));

                cannonBarrelMovable.transform.localPosition = new Vector3(cannonBarrelMovable.transform.localPosition.x, cannonBarrelMovable.transform.localPosition.y,
                                                                        RecoilDistance * (Mathf.Pow((2 * (RecoilTime / RecoilDuration - 0.5f)), 2) - 1));
                

                yield return null;
            }

            cannonBarrelMovable.transform.localPosition = new Vector3(cannonBarrelMovable.transform.localPosition.x, cannonBarrelMovable.transform.localPosition.y, startZ);
            yield break;
        }

        #endregion



    }
}