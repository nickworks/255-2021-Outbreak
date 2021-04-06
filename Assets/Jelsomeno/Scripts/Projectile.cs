using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
{

    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Current speed the player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// life time of projectile 
        /// </summary>
        private float lifespan = 3;

        private float age = 0;

        void Start()
        {

        }
        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }


        void Update()
        {

            age += Time.deltaTime;
            if(age > lifespan)
            {
                Destroy(gameObject);

            }


            RaycastCheck();
            // euler phyics integration:
            transform.position += velocity * Time.deltaTime;

        }

        private void RaycastCheck()
        {
            // make a ray

            Ray ray = new Ray(transform.position, velocity * Time.deltaTime);

            // draw the ray:
            Debug.DrawRay(ray.origin, ray.direction);
        

            // check for collission:
            if( Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude))
            {

               // measuring the movable distance
               if(hit.transform.tag == "Wall")
               {
                    Vector3 normal = hit.normal;

                    normal.y = 0; // stops vertical bounce

                    Vector3 random = Random.onUnitSphere;
                    random.y = 0;

                    normal += random * .5f;

                    normal.Normalize();

                    float allignment = Vector3.Dot(velocity, normal);
                    Vector3 reflection = velocity - 2 * allignment * normal;

                    velocity = reflection;

                    transform.position = hit.point;

                }

            }

        }

        /*
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Wall")
            {
                Vector3 normal = (transform.position - other.transform.position);

                normal.y = 0; // stops vertical bounce

                Vector3 random = Random.onUnitSphere;
                random.y = 0;

                normal += random * .5f;

                normal.Normalize();

                float allignment = Vector3.Dot(velocity, normal);
                Vector3 reflection = velocity - 2 * allignment * normal;

                velocity = reflection;

            }
        }
        */

    }
}
