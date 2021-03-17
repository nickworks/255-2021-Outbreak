using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geib
{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Current Speed and direction the player is moving
        /// </summary>
        private Vector3 velocity;
        /// <summary>
        /// This is how long the projectile should last
        /// </summary>
        private float lifespan = 3;
        /// <summary>
        /// This is how old the progectile is
        /// </summary>
        private float age = 0;
        /// <summary>
        /// This function is called every frame
        /// </summary>
   
        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }
        void Update()
        {
            age += Time.deltaTime * 3;
            if (age > lifespan)
            {
                Destroy(gameObject);
            }

            //euler physics integration:
            transform.position += velocity * Time.deltaTime;

        }
    }
}