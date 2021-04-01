using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves an object in one direction until its time has expired.
/// </summary>
namespace Kortge{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// How many seconds this projectile has been alive, in seconds.
        /// </summary>
        private float age = 0;

        /// <summary>
        /// How long the projectile should live, in seconds.
        /// </summary>
        private float lifespan = 5;

        /// <summary>
        /// Current speed (and direction) the player is moving.
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// Sets the velocity of the projectile.
        /// </summary>
        /// <param name="vel"></param>
        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }

        /// <summary>
        /// Moves the object and destroys it if enough time has passed.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            age += Time.deltaTime;
            if (age > lifespan)
            {
                Destroy(gameObject);
            }
            // euler physics interaction:
            transform.position += velocity * Time.deltaTime;
            RaycastCheck();
        }
        private void RaycastCheck()
        {
            // make a ray:

            Ray ray = new Ray(transform.position, velocity * Time.deltaTime);

            // draw the ray:
            Debug.DrawRay(ray.origin, ray.direction);

            // check for collision:
            /*if(Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude))
            {
                if(hit.transform.CompareTag("Wall"))
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

                    age += (lifespan - age) / 2;
                }
            }*/
            // measuring the movable distance
        }
    }
}