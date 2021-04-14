using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Miller
{
	public class BossHealthBar : MonoBehaviour
	{
		public Slider slider;
		public Gradient gradient;
		public Image fill;

		public void SetMaxHealth(int bossHealth)
		{
			slider.maxValue = bossHealth;
			slider.value = bossHealth;

			fill.color = gradient.Evaluate(1f);
		}

		public void SetHealth(int bossHealth)
		{
			slider.value = bossHealth;

			fill.color = gradient.Evaluate(slider.normalizedValue);
		}
	}
}
