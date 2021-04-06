using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class PlayerHealth : MonoBehaviour
    {
        /// <summary>
        /// Singleton
        /// </summary>
        public static PlayerHealth main;

        /// <summary>
        /// Health state
        /// The getter is public but the setter is private to prevent other classes from effecting it
        /// </summary>
        public float health { get; private set; }

        /// <summary>
        /// Maximum possible health
        /// </summary>
        public float healthMax = 100;

        /// <summary>
        /// Damage cooldown
        /// (i-frames)
        /// </summary>
        private float cooldownInvulnerability = 0;

        private void Start()
        {
            health = healthMax; // sets health to maximum health at startup
        }

        private void Update()
        {
            if (cooldownInvulnerability > 0)
            {
                cooldownInvulnerability -= Time.deltaTime; // if cooldownInvulnerability still has time life, countdown timer
                print(health);
            }
        }

        // Health behavior:
        public void TakeDamage(float amt)
        {
            if (cooldownInvulnerability > 0) return; // still have i-frames, dont take damage
            cooldownInvulnerability = .25f; // cooldown till you can take damage
            if (amt < 0) amt = 0; // Negative numbers ignored
            health -= amt;
            //if (health > 0) SoundEffectBoard.PlayDamage(); // plays damage audio
            if (health <= 0)
            {
                Die(); // if health drops to/below zero do Die method
                //SoundEffectBoard.PlayDie(); // plays death audio
            }
        }

        public void Die()
        {
            Destroy(gameObject); // On death, destroy gameObject
        }
    }
}