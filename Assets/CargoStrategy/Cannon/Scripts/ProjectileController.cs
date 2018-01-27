using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Cannon
{
    public class ProjectileController : MonoBehaviour
    {
        private const float ShotLifespan = 4;

        public GameObject CameraFocus;
        public GameObject CameraTarget;

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
                Debug.Log("Destroy this");
                Destroy(gameObject);
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }


    }

}