using Outbreak;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ASmith
{
    public class EnemyHealth : MonoBehaviour
    {
        /// <summary>
        /// Singleton
        /// </summary>
        public static EnemyHealth main;

        /// <summary>
        /// This is the health variable for all enemies in the game.
        /// The getter for this variable is public
        /// The setter for this variable is private
        /// </summary>
        public float health { get; private set; }

        /// <summary>
        /// The maximum amount of health an enemy can have
        /// Set in the inspector
        /// </summary>
        public float healthMax;

        /// <summary>
        /// The lowest amount of health an enemy can have
        /// Reaching this value should end with the enemy dying
        /// </summary>
        public float healthMin = 0;

        /// <summary>
        /// Timer used to wait a specific amount of time
        /// before going to next level
        /// </summary>
        private float nextLevelTimer = 0;

        /// <summary>
        /// Variable containing the boss gameObject
        /// </summary>
        public GameObject boss;

        /// <summary>
        /// Variable containing the turret gameObject
        /// </summary>
        public GameObject turret;

        void Start()
        {
            health = healthMax; // Sest the player health to the max at start
        }

        void Update()
        {
            if (nextLevelTimer > 0) // If the Timer has been set in the Die() method...
            {
                nextLevelTimer -= Time.deltaTime; // start counting down
            } 
        }

        public void TakeDamage(float amt) // Calculates how much damage to deal to the hit enemy
        {
            amt = GoodBullet.damageAmount;
            if (amt < 0) amt = 0; // Negative numbers ignored

            health -= amt; 

            if (health <= 0) // If hit enemy's health is LESS THAN or EQUAL TO 0...
            {
                Die(); // Enemy dies
            }
        }

        public void Die() // Method runs when an enemy's health reaches or passes 0
        {
            Destroy(gameObject); // Destroys the dead enemy

            if (gameObject == boss) // If dead enemy is the boss...
            {
                SoundBoard.PlayBossDie(); // Play boss death sound

                nextLevelTimer = 4; // Sets the time until the game ends after beating the boss
                if (nextLevelTimer <= 0) // If timer has reached 0
                {
                    Game.GotoNextLevel(); // Go to next level
                }
            }

            if (gameObject == turret) // If dead enemy is a turret...
            {
                SoundBoard.PlayTurretDie(); // Play turret death sound
            }
        }
    }
}