using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves an object in one direction until its time has expired.
/// </summary>
namespace Kortge{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Current speed (and direction) the player is moving.
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// How long the projectile should live, in seconds.
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// How many seconds this projectile has been alive, in seconds.
        /// </summary>
        private float age = 0;

        /// <summary>
        /// Sets the velocity of the projectile.
        /// </summary>
        /// <param name="vel"></param>
        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }

        /// <summary>
        /// Moves the object and destroys it if enough time has passed.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            age += Time.deltaTime;
            if (age > lifespan)
            {
                Destroy(gameObject);
            }
            // euler physics interaction:
            transform.position += velocity * Time.deltaTime;
        }
    }
}