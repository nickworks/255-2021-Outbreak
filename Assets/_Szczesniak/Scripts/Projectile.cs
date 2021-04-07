using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class is used to spawn projectiles that the player shoots
    /// </summary>
    public class Projectile : MonoBehaviour {

        /// <summary>
        /// Current speed (and direction) the player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// How long the projectile should live, in seconds.
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// How many seconds this projectile has been alive, in seconds.
        /// </summary>
        private float age = 0;

        /// <summary>
        /// Damage the the projectiles do
        /// </summary>
        public float damageAmt = 10;

        /// <summary>
        /// Particle effect when the projectiles hit an object
        /// </summary>
        public ParticleSystem bulletParticles;

        /// <summary>
        /// Sets the velocity of the projectile
        /// </summary>
        /// <param name="vel"></param>
        public void InitBullet(Vector3 vel) {
            velocity = vel; // sets velocity
        }

        void Update() {

            age += Time.deltaTime; // counts the age up
            if (age > lifespan) { // if age is greater than lifespan
                Destroy(gameObject); // destroy the projectile
            }

            RayCastCheck(); // does ray cast check to bounce off walls


            // euler physics intergration:
            transform.position += velocity * Time.deltaTime; //moves projectiles.
        }

        /// <summary>
        /// Checks object the ray touches to bounce in an angular way
        /// </summary>
        private void RayCastCheck() {
            // make a Ray:
            Ray ray = new Ray(transform.position, velocity * Time.deltaTime);

            // draw the ray:
            Debug.DrawRay(ray.origin, ray.direction);

            // Check for collision:
            if (Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude)) {
                // measuring the moveable distance
                if (hit.transform.tag == "Wall") {

                    Vector3 normal = hit.normal;
                    normal.y = 0; // no vertical bouncing


                    Vector3 random = Random.onUnitSphere;
                    random.y = 0;

                    // blend the normal with the random:
                    normal += random * .5f;

                    normal.Normalize(); // make unit

                    // causes the reflection
                    float alignment = Vector3.Dot(velocity, normal);
                    Vector3 reflection = velocity - 2 * alignment * normal;



                    velocity = reflection;

                    transform.position = hit.point;
                }
            }
        }

        /// <summary>
        /// When the projectile hits an object
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            HealthScript healthOfThing = other.GetComponent<HealthScript>(); // gets health script of object
            if (healthOfThing) { // if object has a health script
                healthOfThing.DamageTaken(damageAmt); // damages object
                Instantiate(bulletParticles, this.transform.position, Quaternion.identity); // spawns the particle effect
                Destroy(this.gameObject); // destroys the projectile
            }
        }
    }
}