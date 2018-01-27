using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Cannon
{
    public class ProjectileController : MonoBehaviour
    {
        private const float ShotLifespan = 2;

        public GameObject CameraFocus;
        public GameObject CameraTarget;
        public GameObject myModel;

        float timeFired;


        public void Fire(float Power, Transform barrel)
        {
            timeFired = Time.time;
            GetComponent<Rigidbody>().AddForce(Power * barrel.forward, ForceMode.Impulse);
            transform.forward = barrel.forward;
        }

        public void Update()
        {
            if (Time.time - timeFired > ShotLifespan)
            {
                Debug.Log("Destroy model");
                Destroy(myModel);
            }
            if (Time.time - timeFired > ShotLifespan + CargoStrategy.Camera.CameraController.cameraChangeDelay*  1.1f)
            {
                Debug.Log("Destroy GO");
                Destroy(gameObject);
            }
        }



    }

}