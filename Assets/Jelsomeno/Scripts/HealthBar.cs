using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Jelsomeno
{

    public class HealthBar : MonoBehaviour
    {

        public HealthSystem playerHealth;
        public Image fillImage;
        private Slider slider;


        // Start is called before the first frame update
        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (slider.value <= slider.minValue)
            {
                fillImage.enabled = false;
            }
            if (slider.value > slider.minValue && !fillImage.enabled)
            {
                fillImage.enabled = true;
            }

            float fillvalue = playerHealth.health / playerHealth.healthMax;
            if (fillvalue <= slider.maxValue / 3)
            {
                fillImage.color = Color.red;
            }
            else if (fillvalue > slider.maxValue / 3)
            {
                fillImage.color = Color.green;
            }
            slider.value = fillvalue;

        }
    }
}
