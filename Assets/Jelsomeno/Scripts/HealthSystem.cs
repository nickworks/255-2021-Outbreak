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
            health = healthMax;// at start the player has maxhealth
        }
        /// <summary>
        /// if the target is taking damage
        /// </summary>
        /// <param name="amt"></param>
        public void TakeDamage(float amt)
        {
            if (amt <= 0) return;

            health -= amt;

            if (health <= 0) Die(); // if the health of player or boss drops below it health it runs the Die method 
        }

        public void Die()
        {

            Destroy(gameObject);// when player or enemy has no more health then the gameobject is destroyed

        }
    }

}



