using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
{

    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Current speed the player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// life time of projectile 
        /// </summary>
        private float lifespan = 3;

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
            if(age > lifespan)
            {
                Destroy(gameObject);

            }

            // euler phyics integration:
            transform.position += velocity * Time.deltaTime;

        }
    }
}
