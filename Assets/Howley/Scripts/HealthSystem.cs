using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class HealthSystem : MonoBehaviour
    {
        /// <summary>
        /// This variable holds the current health for the player. Can be used from anywhere, but only set in this script.
        /// </summary>
        public float health { get; private set; }

        /// <summary>
        /// This variable holds the maximum health for the object.
        /// </summary>
        public float healthMax = 100;

        /// <summary>
        ///  This varibale is a timer for when the object is able to take damage
        /// </summary>
        private float damageCooldown = 0;

        private void Start()
        {
            health = healthMax;
        }

        /// <summary>
        ///  update is called every game tick
        /// </summary>
        private void Update()
        {
            if (damageCooldown > 0) damageCooldown -= Time.deltaTime;
        }

        /// <summary>
        /// this function gives a damage amount to an object with this script.
        /// </summary>
        /// <param name="dmgAmount"></param>
        public void Damage(float dmgAmount)
        {
            if (dmgAmount < 0) dmgAmount = 0;
            health -= dmgAmount;
            if (health <= 0) Death();
        }

        /// <summary>
        /// When the object's health is <= 0 call the death function to remove it.
        /// </summary>
        public void Death()
        {

        }
    }
}

