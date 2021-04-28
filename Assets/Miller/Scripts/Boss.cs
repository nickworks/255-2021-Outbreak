using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Miller
{
	public class Boss : MonoBehaviour
	{
		// Start is called before the first frame update
		

			public GameObject deathEffect;
			public int maxHealth = 700;

			public int currentHealth;


			public BossHealthBar healthBar;

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



			public void BossTakeDamage(int damage)
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

