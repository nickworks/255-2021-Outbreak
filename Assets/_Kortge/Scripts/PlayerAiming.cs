using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class PlayerAiming : MonoBehaviour
    {
        private Camera cam;
        public Transform debugObject;
        public Projectile prefabProjectile;
        private Animator animator;
        public bool stabbing;
        public int roses = 0;
        public Projectile beamPrefab;
        public GameObject sword1;
        public GameObject sword2;
        public GameObject sword3;
        public GameObject sword4;
        public GameObject sword5;
        public GameObject sword6;

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
                if (roses > 0)
                {
                    Projectile beam = Instantiate(beamPrefab, transform.position + transform.forward, transform.rotation);
                    beam.InitBullet(transform.forward * 20);
                    roses--;
                }
                switch (roses)
                {
                    case 6:
                        sword6.SetActive(false);
                        sword5.SetActive(true);
                        sword4.SetActive(true);
                        sword3.SetActive(true);
                        sword2.SetActive(true);
                        sword1.SetActive(true);
                        break;
                    case 5:
                        sword6.SetActive(false);
                        sword5.SetActive(true);
                        sword4.SetActive(true);
                        sword3.SetActive(true);
                        sword2.SetActive(true);
                        sword1.SetActive(true);
                        break;
                    case 4:
                        sword6.SetActive(false);
                        sword5.SetActive(false);
                        sword4.SetActive(true);
                        sword3.SetActive(true);
                        sword2.SetActive(true);
                        sword1.SetActive(true);
                        break;
                    case 3:
                        sword6.SetActive(false);
                        sword5.SetActive(false);
                        sword4.SetActive(false);
                        sword3.SetActive(true);
                        sword2.SetActive(true);
                        sword1.SetActive(true);
                        break;
                    case 2:
                        sword6.SetActive(false);
                        sword5.SetActive(false);
                        sword4.SetActive(false);
                        sword3.SetActive(false);
                        sword2.SetActive(true);
                        sword1.SetActive(true);
                        break;
                    case 1:
                        sword6.SetActive(false);
                        sword5.SetActive(false);
                        sword4.SetActive(false);
                        sword3.SetActive(false);
                        sword2.SetActive(false);
                        sword1.SetActive(true);
                        break;
                    case 0:
                        sword6.SetActive(false);
                        sword5.SetActive(false);
                        sword4.SetActive(false);
                        sword3.SetActive(false);
                        sword2.SetActive(false);
                        sword1.SetActive(false);
                        break;
                }
            }
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
            if (hit.gameObject.CompareTag("Rose") && roses < 6)
            {
                roses++;
                Destroy(hit.gameObject);
            }

        }
    }
}