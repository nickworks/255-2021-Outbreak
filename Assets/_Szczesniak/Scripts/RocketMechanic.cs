using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class RocketMechanic : MonoBehaviour {

        /// <summary>
        /// Max life of the of the object
        /// </summary>
        private float life = 4;

        /// <summary>
        /// age of the rocket
        /// </summary>
        private float age = 0;

        /// <summary>
        /// Damage that the rocket does
        /// </summary>
        private float damageAmt = 30;


        /// <summary>
        /// Particles for explosion
        /// </summary>
        public ParticleSystem[] explosions;

        /// <summary>
        ///  delete part of rocket
        /// </summary>
        public Transform rocketToDelete;

        /// <summary>
        /// rocket smoke trail
        /// </summary>
        private ParticleSystem smokeTrail;

        /// <summary>
        /// Rocket's collider
        /// </summary>
        Collider rocketsCollider;

        /// <summary>
        /// Runs a part of script once
        /// </summary>
        bool runOnce = true;

        void Start() {
            smokeTrail = GetComponentInChildren<ParticleSystem>(); // Gets ParticleSystem
            rocketsCollider = GetComponent<Collider>(); // Gets Collider
        }

        void Update() {

            age += Time.deltaTime; // Couts up the age
            if (age > life && runOnce) { // if age is greater than life and runOnce is ture
                if (rocketToDelete) Destroy(rocketToDelete.gameObject); // destroys part of rocket
                Destroy(gameObject, 3); // destroy whole rocket in 3 seconds
                rocketsCollider.enabled = false; // turns collider off
                runOnce = false; // makes it false
            }

            if (rocketToDelete) { // if rocketToDelete is true
                transform.position += (transform.forward * 20) * Time.deltaTime; // moves the rocket
            }
        }

        /// <summary>
        /// When the rocket hits a object
        /// </summary>
        /// <param name="other"></param>
        [System.Obsolete]
        private void OnTriggerEnter(Collider other) {
            SplashDamageCheck(); // makes a splash damage collider
            foreach (ParticleSystem eachPart in explosions) { // makes eash particle in list spawn
                Instantiate(eachPart, this.transform.position, Quaternion.Euler(-90, 0, 0));
            }
            if (rocketToDelete) // if true
                Destroy(rocketToDelete.gameObject); // delete part of rocket
            smokeTrail.loop = false; // turn off particle smoke
            rocketsCollider.enabled = false; // turn off collider
        }

        void SplashDamageCheck() {
            Collider[] enemiesHit = Physics.OverlapSphere(transform.position, 10); // checks to see if enemies in a 10 meter distances were hit

            foreach (Collider other in enemiesHit) { 
                HealthScript healthOfThing = other.GetComponent<HealthScript>(); // get enemies health
                if (healthOfThing) { // if there were enemies that have health
                    healthOfThing.DamageTaken(damageAmt); // damages enemies
                }
            }
        }
    }
}