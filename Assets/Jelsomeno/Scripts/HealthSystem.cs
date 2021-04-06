using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
{
    public class HealthSystem : MonoBehaviour
    {
        public float health { get; private set; }
        public float healthMax = 100;

        //public GameObject impactEffect;

        private void Start()
        {
            health = healthMax;
        }

        public void TakeDamage(float amt)
        {
            if (amt <= 0) return;

            health -= amt;

            if (health <= 0) Die();
        }

        public void Die()
        {
            //GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            //Destroy(effectIns, 2f);

            Destroy(gameObject);

        }
    }
}
