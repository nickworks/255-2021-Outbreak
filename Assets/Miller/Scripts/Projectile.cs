using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miller
{
    public class Projectile : MonoBehaviour
    {
        private Vector3 velocity;

        public int damage = 10;

        /// <summary>
        /// How long the projectile should live in seconds
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// How many seconds this projectile has been alive
        /// </summary>
        private float age = 0;


        private void OnTriggerEnter(Collider other)
        {



            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<Boss>().BossTakeDamage(damage);
            }


        }


        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }

        void Update()
        {
            age += Time.deltaTime;
            if (age > lifespan)
            {
                Destroy(gameObject);
            }

            //euler physics intergration;
            transform.position += velocity * Time.deltaTime;
        }
    }
}

