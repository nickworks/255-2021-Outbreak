using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pattison {
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

        void Start() {

        }
        public void InitBullet(Vector3 vel) {

            velocity = vel;
        }

        // Update is called once per frame
        void Update() {

            age += Time.deltaTime;
            if (age > lifespan) {
                Destroy(gameObject);
            }


            // euler physics integration:
            transform.position += velocity * Time.deltaTime;
        }
    }
}