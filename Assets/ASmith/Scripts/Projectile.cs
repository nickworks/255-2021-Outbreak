using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith {
    public class Projectile : MonoBehaviour {
        /// <summary>
        /// Current speed and direction the player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// How long the projectile should live, in seconds
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// How long the projectile has been alive, in seconds
        /// </summary>
        private float age = 0;
        void Start() {

        }

        public void InitBullet(Vector3 vel) {
            velocity = vel;
        }

        void Update() {
            age += Time.deltaTime;
            if (age > lifespan) {
                Destroy(gameObject);
            }

            RaycastCheck();

            // euler physics integration
            transform.position += velocity * Time.deltaTime;


        }

        private void RaycastCheck()
        {
            Ray ray = new Ray(transform.position, velocity * Time.deltaTime);

            Debug.DrawRay(ray.origin, ray.direction);

            if(Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude))
            {
                if (hit.transform.tag == "Wall")
                {
                    print("hit a wall");

                    Vector3 normal = hit.normal;
                    normal.y = 0; // no vertical bouncing

                    Vector3 random = Random.onUnitSphere;
                    random.y = 0;

                    // blend the normal with the random:
                    normal += random * .3f;

                    normal.Normalize(); // makes unit vector

                    float alignment = Vector3.Dot(velocity, normal);
                    Vector3 reflection = velocity - 2 * alignment * normal;

                    reflection = Vector3.Lerp(reflection, Random.onUnitSphere, 0.5f);

                    velocity = reflection;
                    transform.position = hit.point;
                }
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if(other.tag == "Wall")
        //    {
        //        print("hit a wall");

        //        Vector3 normal = (transform.position - other.transform.position);
        //        normal.y = 0; // no vertical bouncing

        //        Vector3 random = Random.onUnitSphere;
        //        random.y = 0; 

        //        // blend the normal with the random:
        //        normal += random * .5f;

        //        normal.Normalize(); // makes unit vector

        //        float alignment = Vector3.Dot(velocity, normal);
        //        Vector3 reflection = velocity - 2 * alignment * normal;

        //        reflection = Vector3.Lerp(reflection, Random.onUnitSphere, 0.5f);

        //        velocity = reflection;
        //    }
        //}
    }
}