using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Terrain;

namespace CargoStrategy.Cannon
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField]
        private GameObject ExplosionPrefab;

        [SerializeField]
        private float m_blastRange = 5.0f;

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

        GameObject EP;

        public event System.Action ProjectileDeletedEvent;

        public void Update()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
            if ((Time.time - timeFired > ShotLifespan) || Collided)
            {
                TimeDestroyed = Time.time - timeFired;
                Destroy(myModel);
                Destroy(rb);

                StartCoroutine(DeleteGO());
            }

        }


        IEnumerator DeleteGO()
        {
            yield return new WaitForSeconds(Camera.CameraController.cameraChangeDelay);
            Destroy(gameObject);
            if (EP)
            {
                Destroy(EP);
            }
            System.Action temp = ProjectileDeletedEvent;
            if (temp != null) { ProjectileDeletedEvent(); }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (!EP)
            {
                EP = Instantiate<GameObject>(ExplosionPrefab, transform.position, Quaternion.identity);
            }

            Collided = true;
            RoadMesh closestMesh = null;
            float closestDistance = -1.0f;
            for (int i = 0; i < RoadMesh.RoadMeshes.Count; i++)
            {
                RoadMesh mesh = RoadMesh.RoadMeshes[i];
                Vector3 closest = GetClosestPointOnLine(mesh.From.Position, mesh.To.Position, transform.position);
                float distance = Vector3.Distance(closest, transform.position);
                if (distance < m_blastRange && (closestMesh == null || closestDistance > distance))
                {
                    closestMesh = mesh;
                    closestDistance = distance;
                }
            }
            if (closestMesh != null)
            {
                closestMesh.DestroyConnection();
            }
        }

        private Vector3 GetClosestPointOnLine(Vector3 start, Vector3 end, Vector3 position)
        {
            Vector3 toStart = start - position;
            Vector3 lineNormal = end - start;
            if (Vector3.Dot(toStart, lineNormal) > 0.0f)
            {
                return start;
            }
            Vector3 toEnd = end - position;
            if (Vector3.Dot(toEnd, lineNormal) < 0.0f)
            {
                return end;
            }
            position -= start;
            return Vector3.Project(position, lineNormal) + start;
        }
        
    }

}