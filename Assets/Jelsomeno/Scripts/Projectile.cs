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
        /// on bullet hit, sparks will fly up using this particle system
        /// </summary>
        public ParticleSystem bulletImpact;

        /// <summary>
        /// life time of projectile 
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// how long the projectile has been alive
        /// </summary>
        private float age = 0;

        void Start()
        {

        }
        /// <summary>
        /// setting the velocity of projectile
        /// </summary>
        /// <param name="vel"></param>
        public void InitBullet(Vector3 vel)
        {
            velocity = vel;// velocity of the bullet 
        }


        void Update()
        {

            age += Time.deltaTime; // counts the age up
            if(age > lifespan) // if the age becomes older then lifespan
            {
                Destroy(gameObject); // destroys the bullet after it lifespan is over 

            }


            RaycastCheck();
            // euler phyics integration:
            transform.position += velocity * Time.deltaTime;

        }

        /// <summary>
        /// created in class to help bullets bounce off walls
        /// </summary>
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

                    normal += random * .5f;// mixes normal with random

                    normal.Normalize(); // make unit

                    float allignment = Vector3.Dot(velocity, normal);
                    Vector3 reflection = velocity - 2 * allignment * normal;

                    velocity = reflection;// cause

                    transform.position = hit.point;

                }

            }

        }

        /// <summary>
        /// projectile is hitting a target, in this case the boss
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter(Collider collision)
        {
            HealthSystem healthOfEnemy = collision.GetComponent<HealthSystem>(); // gets the health script
            if (collision.gameObject.tag == "Boss" && healthOfEnemy)
            {
                healthOfEnemy.DamageTaken(25); // damages the boss

                Instantiate(bulletImpact, this.transform.position, Quaternion.identity); // spawns the particles
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
