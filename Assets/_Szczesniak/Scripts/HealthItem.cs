using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// Manages the health item that the player can pickup
    /// </summary>
    public class HealthItem : MonoBehaviour {

        /// <summary>
        /// Amount of health the item gives the player upon pickup
        /// </summary>
        public float healthIncreaseAmt = 10;

        /// <summary>
        /// Gets the its position
        /// </summary>
        private Vector3 healthItemPos;

        /// <summary>
        /// How fast it rotates
        /// </summary>
        public float rotationSpeed = 20;

        /// <summary>
        /// the amount of bouncing the item does
        /// </summary>
        public float bounceAmt = 2;

        /// <summary>
        /// sets the bounce to go up and down
        /// </summary>
        private float setAmountBounce;

        /// <summary>
        /// Gets the parents transform
        /// </summary>
        Transform parentObj;

        void Start() {
            healthItemPos = transform.localPosition; // gets local position
            setAmountBounce = bounceAmt; // sets the bounce amount
            parentObj = GetComponentInParent<Transform>(); // gets the transform
        }


        void Update() {
            transform.Rotate(0, rotationSpeed, 0); // rotates the health item

            if (transform.localPosition.y <= 0) {
                bounceAmt = setAmountBounce; // goes up
            }

            if (transform.localPosition.y >= .5f) {
                bounceAmt = -setAmountBounce; // goes down
            }

            healthItemPos.y += Mathf.Sin(Time.deltaTime * bounceAmt) * .05f; // does the bounce action

            healthItemPos.y = Mathf.Clamp(healthItemPos.y, -1, 1); // clamps how far can bounce
            transform.localPosition += healthItemPos * Time.deltaTime; // moves the item from the bounces
        }

        private void OnTriggerEnter(Collider other) {
            HealthScript playerHealth = other.GetComponent<HealthScript>();
            if (playerHealth && other.tag == "Player") { // if touched by player
                playerHealth.HealingItemEffect(healthIncreaseAmt); // increase lost health
                Destroy(parentObj.gameObject); // destroys item
            }
        }
    }
}