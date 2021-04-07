using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Jelsomeno
{
    /// <summary>
    /// controls the health bar fills in the UI
    /// </summary>
    public class HealthBar : MonoBehaviour
    {

        public HealthSystem playerHealth; // reference to health system
        public Image fillImage;// image used to fill the health bar
        private Slider slider; // slider reference


        // Start is called before the first frame update
        void Start()
        {
            slider = GetComponent<Slider>();// gets slider component in canvas 
        }

        // Update is called once per frame
        void Update()
        {
            
            if (slider.value <= slider.minValue)
            {
                fillImage.enabled = false; // keep health bar filled
            }
            if (slider.value > slider.minValue && !fillImage.enabled)
            {
                fillImage.enabled = true;  // unfill health bar
            }

            float fillvalue = playerHealth.health / playerHealth.healthMax;
            if (fillvalue <= slider.maxValue / 3)
            {
                fillImage.color = Color.red; // when health gets down to a certain point it turns red 
            }
            else if (fillvalue > slider.maxValue / 3)
            {
                fillImage.color = Color.green; // when health is at a certain point it stays green
            }
            slider.value = fillvalue;

        }
    }
}
