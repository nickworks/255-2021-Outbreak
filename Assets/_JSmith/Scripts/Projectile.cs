using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _JSmith
{
    public class Projectile : MonoBehaviour
    {


        /// <summary>
        /// Current speed (and direction) the player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// HOw long the projectile should live, in seconds
        /// </summary>
        private float lifeSpan = 1;

        /// <summary>
        /// How many seconds this projectile has been alive
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
            if(age > lifeSpan)
            {
                Destroy(gameObject);
            }

            //euler physics integration:
            transform.position += velocity * Time.deltaTime;

        }
    }
}
