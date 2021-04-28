using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    /// <summary>
    /// this projectile is used for the enemy against the player
    /// </summary>
    public class EnemyProjectile : MonoBehaviour
    {
        /// <summary>
        /// Current speed the boss is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// life time of projectile 
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// how long the projectile has been alive
        /// </summary>
        private float age = 0;

        /// <summary>
        /// how much damage the projectile can do
        /// </summary>
        public float damageAmt = 20;

        /// <summary>
        /// on bullet hit, sparks will fly up using this particle system
        /// </summary>
        public ParticleSystem bulletImpact;

        /// <summary>
        /// setting the velocity of projectile
        /// </summary>
        /// <param name="vel"></param>
        public void InitBullet(Vector3 vel)
        {
            velocity = vel; 
        }

        void Update()
        {

            age += Time.deltaTime; // counts the age up
            if (age > lifespan) // if the age becomes older then lifespan
            { 
                Destroy(gameObject); // destroys the bullet after it lifespan is over 
            }


      
            transform.position += velocity * Time.deltaTime; // euler phyics integration:
        }

        /// <summary>
        /// created in class to help bullets bounce off walls
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            HealthSystem healthOfThing = other.GetComponent<HealthSystem>(); // making a local variable from getting access from HealthScript
            if (healthOfThing)
            { // if the healthOfThing has data/storing something
                healthOfThing.DamageTaken(damageAmt); // damages the target
            }

            Instantiate(bulletImpact, this.transform.position, Quaternion.identity); // spawns the particles
            Destroy(this.gameObject); // destroys the projectile
            
        }
    }
}
