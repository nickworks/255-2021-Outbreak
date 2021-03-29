using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kortge
{
    public class PlayerAiming : MonoBehaviour
    {
        private Camera cam;
        public Transform debugObject;
        public Projectile prefabProjectile;
        private Animator animator;
        public bool stabbing;
        public bool projectionReady = false;
        public Projectile beamPrefab;
        public RawImage sword;
        private float blueness = 0;

        // Start is called before the first frame update
        void Start()
        {
            cam = Camera.main;
            animator = GetComponentInChildren<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            AimAtMouse();
            if (Input.GetButtonDown("Fire1") && !stabbing)
            {
                animator.SetTrigger("Stab");
                if (projectionReady)
                {
                    Projectile beam = Instantiate(beamPrefab, transform.position + transform.forward, transform.rotation);
                    beam.InitBullet(transform.forward * 20);
                    projectionReady = false;
                }
            }
            if (projectionReady) blueness += Time.deltaTime;
            else blueness -= Time.deltaTime * 5;
            blueness = Mathf.Clamp(blueness, 0, 1);
            sword.color = Color.Lerp(Color.white, Color.blue, blueness);
        }

        private void AimAtMouse()
        {
            // make a ray and a plane:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {
                // find where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI;
                angle *= 180; // Converts from radians to half-circles to degrees.
                transform.eulerAngles = new Vector3(0, angle, 0);
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Rose") && !projectionReady)
            {
                projectionReady = true;
                Destroy(hit.gameObject);
            }

        }
    }
}