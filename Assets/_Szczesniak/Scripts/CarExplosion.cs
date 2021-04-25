using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class CarExplosion : MonoBehaviour {

        public ParticleSystem explosion;
        public Transform carObj;

        private HealthScript carHealth;

        public float damageAmt = 50;

        void Start() {
            carHealth = GetComponent<HealthScript>();
        }


        void Update() {

            if (carHealth.health <= 0) DestroyedCar();
        }

        void DestroyedCar() {
            Instantiate(explosion, transform.position, Quaternion.Euler(-90, 0, 0));
            Instantiate(carObj, transform.position, transform.rotation);

            SplashDamage();

            Destroy(gameObject);
        }

        void SplashDamage() {
            Collider[] pawnsHit = Physics.OverlapSphere(transform.position, 7);

            foreach (Collider other in pawnsHit) {
                HealthScript healthOfThing = other.GetComponent<HealthScript>();
                if (healthOfThing) {
                    healthOfThing.DamageTaken(damageAmt);
                }
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (carHealth && other.tag == "Bullet") {
                carHealth.DamageTaken(20);
            }
        }
    }
}