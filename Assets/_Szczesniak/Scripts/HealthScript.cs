using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Szczesniak {
    /// <summary>
    /// Class for everything that has health
    /// </summary>
    public class HealthScript : MonoBehaviour {

        /// <summary>
        /// health of the object
        /// </summary>
        public float health { get; private set; }
        
        /// <summary>
        /// sets the health
        /// </summary>
        public float maxHealth = 100; 

        /// <summary>
        /// Slider for the health bar
        /// </summary>
        public Slider healthSlider;

        void Start() {
            health = maxHealth; // set health
            HealthBarSetup(); // Sets the health bar
        }

        void HealthBarSetup() {
            healthSlider.maxValue = health; // sets the slider's max value
            healthSlider.value = health; // Sets the health
        }

        void CurrentHealth() {
            healthSlider.value = health; // sets health throughout the object's life from being updated
        }

        /// <summary>
        /// When the object has been damaged
        /// </summary>
        /// <param name="damage"></param>
        public void DamageTaken(float damage) {
            health -= damage; // takes health away from damage

            CurrentHealth(); // sets the health for the health bar

            if (health <= 0) {
                Destroy(this.gameObject, 3); // destroys the game object in 3 seconds
                SoundEffectBoard.PlayDeathSound(); // plays the deathSound
            }
        }
    }
}