using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class MissleScript : MonoBehaviour {

        private Vector3 velocity;

        public float rotationAmt = 5;
        public float speed = 10;
        public float launchAngle = 40f;

        private float life = 4;

        private float age = 0;

        public float damageAmt = 25;

        public Transform target;

        public bool timeToLaunch = false;

        public ParticleSystem missleParticles;

        private void Start() {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update() {

            if (!timeToLaunch)
                return;

            age += Time.deltaTime;

            if (age > life) {
                Destroy(gameObject);
            }

            transform.parent = null;


            MoveTowardsTarget();


        }

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

            Vector3 targetLocation = target.position - transform.position;
            Vector3 directionToGo = Vector3.RotateTowards(transform.forward, targetLocation, rotationAmt * Time.deltaTime, 0.0f);

            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);

            transform.rotation = Quaternion.LookRotation(directionToGo);
        }

        public void InitMissle(Vector3 setVelocity) {
            velocity = setVelocity;
        }

        private void OnTriggerEnter(Collider other) {
            HealthScript healthOfThing = other.GetComponent<HealthScript>();
            if (healthOfThing) {
                healthOfThing.DamageTaken(damageAmt);
            }

            Instantiate(missleParticles, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}