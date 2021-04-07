using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// Class for the missle to go to their target
    /// </summary>
    public class MissleScript : MonoBehaviour {

        /// <summary>
        /// how fast the missile should go
        /// </summary>
        private Vector3 velocity; 

        /// <summary>
        /// how much the missile can turn
        /// </summary>
        public float rotationAmt = 5;

        /// <summary>
        /// how fast the missile is 
        /// </summary>
        public float speed = 10;

        /// <summary>
        /// the launch angle of the missile
        /// </summary>
        public float launchAngle = 40f;

        /// <summary>
        /// Life of the missile
        /// </summary>
        private float life = 4;

        /// <summary>
        /// how long it has left to 'live'
        /// </summary>
        private float age = 0;

        /// <summary>
        /// the amount of damage it can do
        /// </summary>
        public float damageAmt = 25;

        /// <summary>
        /// Target the missile is going towards
        /// </summary>
        public Transform target;

        /// <summary>
        /// When to launch
        /// </summary>
        public bool timeToLaunch = false;

        /// <summary>
        /// Particle effect when the missile explodes
        /// </summary>
        public ParticleSystem missleParticles;

        private void Start() {
            target = GameObject.FindGameObjectWithTag("Player").transform; // finds the player
        }

        void Update() {

            if (!timeToLaunch) // if timeToLaunch is false, won't run
                return;

            age += Time.deltaTime; // increase age 

            if (age > life) { // if age is greater than life
                Destroy(gameObject); // destroy the missile
            }

            transform.parent = null; // missile removes itself from the tank/boss

            // Method that moves the missile
            MoveTowardsTarget();


        }

        /// <summary>
        /// Moves the missile towards the target
        /// </summary>
        private void MoveTowardsTarget() {
            /*
            Vector3 misslePos = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 targetPos = new Vector3(target.position.x, 0f, transform.position.z);
            transform.LookAt(targetPos);

            float r = Vector3.Distance(misslePos, targetPos);
            float g = Physics.gravity.y;
            float tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
            float h = target.position.y - transform.position.y;

            float velocityZ = Mathf.Sqrt(g * r * r / (2f * (h - r * tanAlpha)));
            float velocityY = tanAlpha * velocityZ;

            Vector3 localVel = new Vector3(velocityZ, velocityY, velocityZ);
            Vector3 globalVel = transform.TransformDirection(localVel);

            rigidMissle.velocity = globalVel; */

            Vector3 targetLocation = target.position - transform.position; // gets the location of the target
            // gets direction to know where to go
            Vector3 directionToGo = Vector3.RotateTowards(transform.forward, targetLocation, rotationAmt * Time.deltaTime, 0.0f);

            // moves the missile to the target
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);

            // turns the missile towards the target
            transform.rotation = Quaternion.LookRotation(directionToGo);
        }

        /// <summary>
        /// Sets the velocity
        /// </summary>
        /// <param name="setVelocity"></param>
        public void InitMissle(Vector3 setVelocity) {
            velocity = setVelocity; // sets the velocity
        }

        /// <summary>
        /// When the missile hits the target
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            HealthScript healthOfThing = other.GetComponent<HealthScript>(); // gets the HealthScript
            if (healthOfThing) { // if the healthOfThing is 'there'
                healthOfThing.DamageTaken(damageAmt); // damages the target
            }

            Instantiate(missleParticles, this.transform.position, Quaternion.identity); // spawns the particle effect of the explosion
            Destroy(this.gameObject); // Destroys gameObject
        }
    }
}