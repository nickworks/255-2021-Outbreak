
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Current direction/speed
        /// </summary>
        private Vector3 velocity;

        private BossStates boss;

        /// <summary>
        /// How long until the projectile is destroyed.
        /// </summary>
        private float lifeSpan = 3;

        /// <summary>
        /// How long the projectile has lived.
        /// </summary>
        private float age = 0;

        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }

        void Update()
        {
            age += Time.deltaTime;
            if (age > lifeSpan)
            {
                Destroy(gameObject);
            }

            RaycastCheck();

            // Euler physics
            transform.position += velocity * Time.deltaTime;

        }

        /// <summary>
        /// Thic function shoots a ray forward from the object, and checks for collision.
        /// </summary>
        private void RaycastCheck()
        {
            // Make a Ray
            Ray ray = new Ray(transform.position, velocity * Time.deltaTime);

            // Draw the Ray:
            Debug.DrawRay(ray.origin, ray.direction);

            // Check for collision
            if (Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude))
            {
                 // Measure the moveable distance
                 if (hit.transform.tag == "Wall")
                 {
                    Vector3 normal = hit.normal;
                    normal.y = 0;

                    Vector3 random = Random.onUnitSphere;

                    normal += random * .5f;

                    normal.Normalize();

                    float alignment = Vector3.Dot(velocity, normal);
                    Vector3 reflection = velocity - 2 * alignment * normal;

                    velocity = reflection;

                    transform.position = hit.point;
                 }
            }

        }
    }
}

