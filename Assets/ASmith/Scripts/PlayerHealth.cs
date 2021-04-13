using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        public static float health { get; private set; }

        /// <summary>
        /// Maximum possible health
        /// </summary>
        public float healthMax = 100;

        /// <summary>
        /// Minimum possible health
        /// </summary>
        private float healthMin = 0;

        /// <summary>
        /// Damage cooldown
        /// (i-frames)
        /// </summary>
        private float cooldownInvulnerability = 0;

        /// <summary>
        /// Current amount of health the shield has
        /// </summary>
        public float currShieldHealth = 30;

        /// <summary>
        /// The minimum amount of health the shield can have
        /// </summary>
        public float minShieldHealth = 0;

        /// <summary>
        /// The minimum amount of shield health required for the shield to activate
        /// </summary>
        public float minUsableShieldHealth = 10;

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

        /// <summary>
        /// Whether or not the player's shield is active
        /// </summary>
        private bool shielding = false;

        public GameObject player;
        public GameObject Shield;
        private MeshRenderer shieldRender;

        public Image healthFillImage;
        public Text healthDisplayText;

        public Image shieldFillImage;
        //public Text shieldDisplayText;


        // Tracks player health and communicates it to the UI
        public float healthValue
        {
            get
            {
                return health;
            }
            set
            {
                // Clamps the passed value within min/max range
                health = Mathf.Clamp(value, healthMin, healthMax);

                // Calculates the current fill percentage and displays it
                float fillPercentage = health / healthMax;
                healthFillImage.fillAmount = fillPercentage;
                healthDisplayText.text = (fillPercentage * 100).ToString("0") + "%";
            }
        }

        // Tracks player shield health and communicates it to the UI
        public float shieldValue
        {
            get
            {
                return currShieldHealth;
            }
            set
            {
                // Clamps the passed value within min/max range
                currShieldHealth = Mathf.Clamp(value, minShieldHealth, maxShieldHealth);

                // Calculates the current fill percentage and displays it
                float fillPercentage = currShieldHealth / maxShieldHealth;
                shieldFillImage.fillAmount = fillPercentage;
                //shieldDisplayText.text = (fillPercentage * 100).ToString("0") + "%";
            }
        }

        private void Start()
        {
            health = healthMax; // sets health to maximum health at startup
            currShieldHealth = maxShieldHealth; // sets shield health to maximum health at startup

            shieldRender = Shield.GetComponent<MeshRenderer>();

            healthValue = health;
            shieldValue = currShieldHealth;
        }

        private void Update()
        {
            print("Current Health: " + health);
            print("Current ShieldHealth: " + currShieldHealth);
            print("Current State: " + currentHealthState);

            healthValue = health;
            shieldValue = currShieldHealth;

            if (cooldownInvulnerability > 0)
            {
                cooldownInvulnerability -= Time.deltaTime; // if cooldownInvulnerability still has time life, countdown timer
            }

            switch (currentHealthState)
            {
                case HealthState.Regular:
                    // Do behavior for this state:
                    shieldRender.enabled = false; // disable the shield render on the player
                    if (currShieldHealth < maxShieldHealth) // If NOT shielding and shieldHealth < max...
                    {
                        currShieldHealth = currShieldHealth + .05f; // Regen shield health 
                    }

                    if (currShieldHealth >= minShieldHealth) // If shieldHealth >= minimumHealth...
                    {
                        canShield = true; // Player can turn on shield...
                    } else { canShield = false; } // ELSE, player can NOT shield

                    // Transition to other states:
                    if (!shielding && Input.GetButtonDown("Shield") && currShieldHealth > minUsableShieldHealth) // If NOT shielding, pressing Q, and currShieldHealth > minShieldHealth...
                    {
                        currentHealthState = HealthState.Shielding; // switch to shielding state
                        shielding = true; // set shielding to true
                    }

                    break;

                case HealthState.Shielding:
                    // Do behavior for this state:
                    shieldRender.enabled = true; // render the shield on the player
                    // Transition to other states:
                    if (shielding && Input.GetButtonDown("Shield")) // If shielding and pressing Q
                    {
                        currentHealthState = HealthState.Regular; // switch to regular state
                        shielding = false; // set shielding to false
                    }
                    break;
            }
        }

        // Health behavior:
        public void TakeDamage(float amt)
        {
            if (cooldownInvulnerability > 0) return; // still have i-frames, dont take damage
            cooldownInvulnerability = .25f; // cooldown till you can take damage
            amt = BadBullet.damageAmount;
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
            healthValue = 0f;
            Destroy(gameObject); // On death, destroy gameObject
        }
    }
}