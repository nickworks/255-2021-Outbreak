using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class is for the enemies' projectiels when they shoot out
    /// </summary>
    public class EnemyProjectiles : MonoBehaviour {
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
        /// Amount of damage it deals to targets
        /// </summary>
        public float damageAmt = 10;

        /// <summary>
        /// Partilce effect when the projectile hits an object
        /// </summary>
        public ParticleSystem bulletParticles;

        /// <summary>
        /// Sets bullet velocity
        /// </summary>
        /// <param name="vel"></param>
        public void InitBullet(Vector3 vel) {
            velocity = vel; // setting velocity
        }

        void Update() {

            age += Time.deltaTime; // adding age as time passes
            if (age > lifespan) { // if age is greater than lifespan
                Destroy(gameObject); // destroy projectile
            }


            // euler physics intergration:
            transform.position += velocity * Time.deltaTime; // moves the projectile forward
        }

        /// <summary>
        /// When the projectile hits a target
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            HealthScript healthOfThing = other.GetComponent<HealthScript>(); // making a local variable from getting access from HealthScript
            if (healthOfThing) { // if the healthOfThing has data/storing something
                healthOfThing.DamageTaken(damageAmt); // damages the target
            }

            Destroy(this.gameObject); // destroys the projectile
            Instantiate(bulletParticles, this.transform.position, Quaternion.identity); // instantiates the bullet particles when it hits an object
        }
    }
}