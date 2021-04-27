using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jelsomeno
{
    /// <summary>
    /// this class handles the health for both the boss and player
    /// </summary>
    public class HealthSystem : MonoBehaviour
    {
        /// <summary>
        /// health of what ever object/player/AI that is linked to this script
        /// </summary>
        public float health { get; private set; }

        /// <summary>
        /// max health that can be changed in the inspector
        /// </summary>
        public float maxHealth = 100;

        /// <summary>
        /// referencet to the slider bars in the UI
        /// </summary>
        public Slider healthSlider;

        void Start()
        {
            health = maxHealth; // set the health
            HealthBarSetup(); // health bar is ready
        }

        void HealthBarSetup()
        {
            healthSlider.maxValue = health; // health bar is full
            healthSlider.value = health; // health bar can change
       }

        void CurrentHealth()
        {
            healthSlider.value = health; // gets the objects life throughout the game
        }

        /// <summary>
        /// object is taking damage
        /// </summary>
        /// <param name="damage"></param>
        public void DamageTaken(float damage)
        {
            health -= damage; // losing health
            CurrentHealth(); // constantly setting health on the health bars

            if (health <= 0)
            {
                Destroy(this.gameObject); // destroys the game object once it loses all its health

            }
        }
    }

}



