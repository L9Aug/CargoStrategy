using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CargoStrategy.Cannon
{
    public class ProjectileController : MonoBehaviour
    {
        private const float ShotLifespan = 10;

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

        float TimeDestroyed;
        bool Collided = false;

        public event System.Action ProjectileDeletedEvent;

        public void Update()
        {
            if ((Time.time - timeFired > ShotLifespan) || Collided)
            {
                TimeDestroyed = Time.time - timeFired;
                Destroy(myModel);
                Destroy(GetComponent<Rigidbody>());
                StartCoroutine(DeleteGO());
            }

        }


        IEnumerator DeleteGO()
        {
            yield return new WaitForSeconds(Camera.CameraController.cameraChangeDelay);
            Destroy(gameObject);

            System.Action temp = ProjectileDeletedEvent;
            if (temp != null) { ProjectileDeletedEvent(); }
        }

        public void OnCollisionEnter(Collision collision)
        {
            Collided = true;
        }


    }

}