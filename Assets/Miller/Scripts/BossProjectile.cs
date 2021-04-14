using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miller { 
public class BossProjectile : MonoBehaviour
{
    
    
        private Vector3 velocity;

        public int damage = 25;

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
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<Player>().TakeDamage(damage);
            }


        }



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
            if (age > lifespan)
            {
                Destroy(gameObject);
            }

            //euler physics intergration;
            transform.position += velocity * Time.deltaTime;
        }
    }
}

