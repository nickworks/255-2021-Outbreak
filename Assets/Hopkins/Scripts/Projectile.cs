using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hopkins
{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// current speed & direction player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// projectile total lifespan in seconds
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// projectile age in seconds
        /// </summary>
        private float age = 0;

        void Start()
        {

        }
        public void InitBullet(Vector3 vel)
        {

            velocity = vel;
        }

        // Update is called once per frame
        void Update()
        {

            age += Time.deltaTime;
            if (age > lifespan)
            {
                Destroy(gameObject);
            }


            // euler physics integration
            transform.position += velocity * Time.deltaTime;
        }
    }
}