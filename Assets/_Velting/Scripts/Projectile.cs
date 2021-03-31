using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Velting
{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Current direction/speed
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// How long until the projectile is destroyed.
        /// </summary>
        private float lifeSpan = 3;

        /// <summary>
        /// How long the projectile has lived.
        /// </summary>
        private float age = 0;

        void Start()
        {

        }
        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }

        void Update()
        {
            age += Time.deltaTime;
            if (age > lifeSpan)
            {
                Destroy(gameObject);
            }

            // Euler physics
            transform.position += velocity * Time.deltaTime;

        }
    }
}
