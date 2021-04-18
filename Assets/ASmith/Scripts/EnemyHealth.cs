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

        public static float health;

        public float healthMax;

        public float healthMin = 0;

        public GameObject boss;

        public GameObject turret;

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
                //float fillPercentage = health / healthMax;
                //healthFillImage.fillAmount = fillPercentage;
                //healthDisplayText.text = (fillPercentage * 100).ToString("0") + "%";
            }
        }

        void Start()
        {
            health = healthMax;
        }

        void Update()
        {
            healthValue = health;
            print(boss + ": " + health);
        }

        public void TakeDamage(float amt)
        {
            amt = BadBullet.damageAmount;
            if (amt < 0) amt = 0; // Negative numbers ignored

            health -= amt;

            if (health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            healthValue = 0f;
            Destroy(gameObject);
        }
    }
}