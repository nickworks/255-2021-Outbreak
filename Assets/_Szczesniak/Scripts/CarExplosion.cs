using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// Car explodes when player or boss shoots at a 'lving' one
    /// </summary>
    public class CarExplosion : MonoBehaviour {

        /// <summary>
        /// explosion particle effect when car explodes
        /// </summary>
        public ParticleSystem explosion;

        /// <summary>
        /// Gets the car itself
        /// </summary>
        public Transform carObj;

        /// <summary>
        /// Health of the car
        /// </summary>
        private HealthScript carHealth;

        /// <summary>
        /// Damage the car can deal.
        /// </summary>
        public float damageAmt = 50;

        void Start() {
            carHealth = GetComponent<HealthScript>(); // gets health script
        }


        void Update() {

            if (carHealth.health <= 0) DestroyedCar(); // if car has no health
        }

        /// <summary>
        /// executes the car explosion and death
        /// </summary>
        void DestroyedCar() {
            Instantiate(explosion, transform.position, Quaternion.Euler(-90, 0, 0)); // runs explosion particle effect
            Instantiate(carObj, transform.position, transform.rotation); // spawns destroyed car

            SplashDamage(); // starts splash damage to things

            Destroy(gameObject); // destroys old car
        }

        /// <summary>
        /// Makes the splash damage work and damage things around it
        /// </summary>
        void SplashDamage() {
            Collider[] pawnsHit = Physics.OverlapSphere(transform.position, 7); // things that were hit in list

            foreach (Collider other in pawnsHit) { 
                HealthScript healthOfThing = other.GetComponent<HealthScript>();
                if (healthOfThing && healthOfThing.health > 0) {
                    healthOfThing.DamageTaken(damageAmt); // damage thing 
                }
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (carHealth && other.tag == "Bullet") {
                carHealth.DamageTaken(20); // damage car if car has health and a bullet hit it
            }
        }
    }
}