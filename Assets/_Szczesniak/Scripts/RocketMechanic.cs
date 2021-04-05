using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class RocketMechanic : MonoBehaviour {

        private float life = 4;
        private float age = 0;
        private float damageAmt = 30;

        public ParticleSystem[] explosions;
        public Transform rocketToDelete;

        private ParticleSystem smokeTrail;

        Collider rocketsCollider;

        bool runOnce = true;

        void Start() {
            smokeTrail = GetComponentInChildren<ParticleSystem>();
            rocketsCollider = GetComponent<Collider>();
        }

        void Update() {

            age += Time.deltaTime;
            if (age > life && runOnce) {
                if (rocketToDelete) Destroy(rocketToDelete.gameObject);
                Destroy(gameObject, 3);
                rocketsCollider.enabled = false;
                runOnce = false;
            }

            if (rocketToDelete) {
                transform.position += (transform.forward * 20) * Time.deltaTime;
            }
        }

        [System.Obsolete]
        private void OnTriggerEnter(Collider other) {
            SplashDamageCheck();
            foreach (ParticleSystem eachPart in explosions) {
                Instantiate(eachPart, this.transform.position, Quaternion.Euler(-90, 0, 0));
            }
            if (rocketToDelete)
                Destroy(rocketToDelete.gameObject);
            smokeTrail.loop = false;
        }

        void SplashDamageCheck() {
            Collider[] enemiesHit = Physics.OverlapSphere(transform.position, 10);

            foreach (Collider other in enemiesHit) {
                HealthScript healthOfThing = other.GetComponent<HealthScript>();
                if (healthOfThing) {
                    healthOfThing.DamageTaken(damageAmt);
                }
            }
        }
    }
}