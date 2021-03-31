using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kortge
{
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// When the character become invulnerable and its sprite starts blinking repeatedly.
        /// </summary>
        public bool postHit;
        /// <summary>
        /// When the character's sprite becomes invisible.
        /// </summary>
        private bool transparent;
        /// <summary>
        /// For the boss, when it is able to be attacked. The player is always vulnerable.
        /// </summary>
        public bool vulnerable;
        /// <summary>
        /// How much time is left until post hit invulnerability has ended.
        /// </summary>
        private float postHitTime = 1;
        /// <summary>
        /// How many hits this character can take before dying.
        /// </summary>
        public int health;
        /// <summary>
        /// The initial tint of the character sprite.
        /// </summary>
        private Color color;
        /// <summary>
        /// The UI elements used to represent the player's health.
        /// </summary>
        public GameObject[] roses = new GameObject[5];
        /// <summary>
        /// A spray of particles that emits when this character is hit.
        /// </summary>
        public ParticleSystem blood;
        /// <summary>
        /// A particle system that is played when the character is killed.
        /// </summary>
        public ParticleSystem particles;
        public Results results;
        /// <summary>
        /// The sprite user to represent the player character.
        /// </summary>
        private SpriteRenderer sprite;

        /// <summary>
        /// Gets the particle system component and the sprite component while also setting the 
        /// </summary>
        // Start is called before the first frame update
        void Start()
        {
            blood = GetComponentInChildren<ParticleSystem>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            color = sprite.color;
        }

        /// <summary>
        /// Keeps the health bar updating while checking if the character is hit or dead.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            UpdateHealthBar();
            if (health <= 0)
            {
                Death();
            }
            else if (postHit && health > 0)
            {
                PostHitInvulnerability();
            }
        }

        private void Death()
        {
            Instantiate(particles, transform.position, transform.rotation);
            PlayerMovement player = GetComponent<PlayerMovement>();
            if (player != null) results.ResultsIn(false);
            else results.ResultsIn(true);
            Destroy(gameObject);
        }

        /// <summary>
        /// Makes the player invulnerable for a limited amount of time, indicated by a blinking sprite.
        /// </summary>
        private void PostHitInvulnerability()
        {
            postHitTime -= Time.deltaTime;
            if (transparent)
            {
                sprite.color = color;
                transparent = false;
            }
            else
            {
                sprite.color = Color.clear;
                transparent = true;
            }
            if (postHitTime <= 0)
            {
                sprite.color = color;
                postHitTime = 1;
                postHit = false;
            }
        }
        /// <summary>
        /// Deactivates a rose on the health bar for every hit taken.
        /// </summary>
        private void UpdateHealthBar()
        {
            int roseIndex = 0;
            foreach (GameObject rose in roses)
            {
                roseIndex++;
                if (roseIndex > health) rose.SetActive(false);
            }
        }
        /// <summary>
        /// Reduces the player's health, sets post-hit invulnerability, and plays the "blood" prefab.
        /// </summary>
        public void Damage()
        {
            if (!postHit && vulnerable)
            {
                health--;
                postHit = true;
                Boss boss = GetComponent<Boss>();
                if (boss != null) boss.hit = true;
                blood.Play();
            }
        }
    }
}