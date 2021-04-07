using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    public class Bullet3 : MonoBehaviour
    {
        private Transform target;

        public float speed = 70f;
        public GameObject impactEffect;


        public void Seek(Transform _target)
        {
            target = _target;
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }

            transform.Translate(dir.normalized * distanceThisFrame, Space.World);

            void HitTarget()
            {
                GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(effectIns, 2f);

                if (target)
                {
                    HealthSystem playerHealth = target.GetComponent<HealthSystem>();
                    if (playerHealth)
                    {
                        playerHealth.TakeDamage(10);
                    }
                    Destroy(gameObject);
                }

            }

        }
    }
}
