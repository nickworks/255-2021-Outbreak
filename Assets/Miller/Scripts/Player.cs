using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miller
{
	public class Player : MonoBehaviour
	{
		public GameObject deathEffect;
		public int maxHealth = 100;
		public int currentHealth;

		public HealthBar healthBar;

		// Start is called before the first frame update
		void Start()
		{
			currentHealth = maxHealth;
			healthBar.SetMaxHealth(maxHealth);
		}

		// Update is called once per frame
		void Update()
		{
			
		}

		public void TakeDamage(int damage)
		{
			currentHealth -= damage;

			healthBar.SetHealth(currentHealth);


			if (currentHealth <= 0)
			{
				Die();
			}
		}

		void Die()
		{
			Instantiate(deathEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
