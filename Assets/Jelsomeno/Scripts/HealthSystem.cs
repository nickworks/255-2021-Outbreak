using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jelsomeno
{
    public class HealthSystem : MonoBehaviour
    {

        public float health { get; private set; }

        public float maxHealth = 100;

        public Slider healthSlider;

        void Start()
        {
            health = maxHealth; 
            HealthBarSetup(); 
        }

        void HealthBarSetup()
        {
            healthSlider.maxValue = health; 
            healthSlider.value = health; 
       }

        void CurrentHealth()
        {
            healthSlider.value = health; 
        }

        public void DamageTaken(float damage)
        {
            health -= damage; 
            CurrentHealth(); 

            if (health <= 0)
            {
                Destroy(this.gameObject); 

            }
        }
    }

}



