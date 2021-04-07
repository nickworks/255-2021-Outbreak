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

        public enum HealthState
        {
            Regular, // 0
            Shielding // 1
        }

        HealthState currentHealthState = HealthState.Regular;

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

        public float currShieldHealth = 30;

        /// <summary>
        /// The minimum amount of health required for the shield to activate
        /// </summary>
        public float minShieldHealth = 10;

        /// <summary>
        /// The maximum amount of health the shield can have
        /// </summary>
        public float maxShieldHealth = 30;

        /// <summary>
        /// Whether or not player can use shield ability
        /// </summary>
        private bool canShield = false;

        /// <summary>
        /// Whether or not damage has been taken
        /// </summary>
        private bool damageTaken = false;

        private bool shielding = false;

        public GameObject Shield;
        private MeshRenderer shieldRender;

        private void Start()
        {
            health = healthMax; // sets health to maximum health at startup
            currShieldHealth = maxShieldHealth; // sets shield health to maximum health at startup

            shieldRender = Shield.GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            print("Current Health: " + health);
            print("Current ShieldHealth: " + currShieldHealth);
            print("Current State: " + currentHealthState);

            if (cooldownInvulnerability > 0)
            {
                cooldownInvulnerability -= Time.deltaTime; // if cooldownInvulnerability still has time life, countdown timer
            }

            switch (currentHealthState)
            {
                case HealthState.Regular:
                    // Do behavior for this state:
                    shieldRender.enabled = false;
                    if (currShieldHealth < maxShieldHealth) // If NOT shielding and shieldHealth < max...
                    {
                        currShieldHealth++; // Regen shield health
                    }

                    if (currShieldHealth >= minShieldHealth) // If shieldHealth >= minimumHealth...
                    {
                        canShield = true; // Player can turn on shield...
                    } else { canShield = false; } // ELSE, player can NOT shield

                    // Transition to other states:
                    if (!shielding && Input.GetButton("Shield") && currShieldHealth > minShieldHealth)
                    {
                        currentHealthState = HealthState.Shielding;
                        shielding = true;
                    }

                    break;

                case HealthState.Shielding:
                    // Do behavior for this state:
                    shieldRender.enabled = true;
                    // Transition to other states:
                    if (shielding && Input.GetButton("Shield"))
                    {
                        currentHealthState = HealthState.Regular;
                        shielding = false;
                    }
                    break;
            }
        }

        // Health behavior:
        public void TakeDamage(float amt)
        {
            if (cooldownInvulnerability > 0) return; // still have i-frames, dont take damage
            cooldownInvulnerability = .25f; // cooldown till you can take damage
            if (amt < 0) amt = 0; // Negative numbers ignored
            damageTaken = true; // Tells the game that the player took damage

            if (shielding) // If player is shielding...
            {
                currShieldHealth -= amt; // Deal damage to the shield
                if (currShieldHealth < 1) // If shield health is below 1...
                {
                    currentHealthState = HealthState.Regular; // Switch back to Regular HealthState
                    shielding = false; // Turn off shield
                }
            }
            else { health -= amt; } // If not shielding, deal damage to player health
            
            //if (health > 0) SoundEffectBoard.PlayDamage(); // plays damage audio
            if (health <= 0)
            {
                Die(); // if health drops to/below zero do Die method
                //SoundEffectBoard.PlayDie(); // plays death audio
            }
            damageTaken = false; // Player no longer taking damage
        }

        public void Die()
        {
            Destroy(gameObject); // On death, destroy gameObject
        }
    }
}