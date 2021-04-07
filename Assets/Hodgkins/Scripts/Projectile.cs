using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins {
    public class Projectile : MonoBehaviour {
        /// <summary>
        /// Current speed (and direction) the player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// How long the projectile should live, in seconds
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// How many seconds this projectile has been alive, in seconds
        /// </summary>
        private float age = 0;

        void Start() {

        }

        public void InitBullet(Vector3 vel) {
            velocity = vel;
        }


        // Update is called once per frame
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
            // make a ray 
            Ray ray = new Ray(transform.position, velocity);
            
            // check for collision
            if(Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude))
            {
                // measuring the movable distance 
                if(hit.transform.tag == "Wall")
                {
                    Vector3 normal = hit.normal;
                    normal.y = 0;

                    Vector3 random = Random.onUnitSphere;
                    random.y = 0;

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