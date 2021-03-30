using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    /// <summary>
    /// Reduces the health of either the player or the boss.
    /// </summary>
    public class Damage : MonoBehaviour
    {
        /// <summary>
        /// Damages the player if true or the boss if false.
        /// </summary>
        public bool player;
        /// <summary>
        /// When collision occurs, this script checks if it is the player or the boss.
        /// If it is, then it reduces its health by one.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if ((player && other.CompareTag("Player"))||(!player && other.CompareTag("Boss")))
            {
                Health health = other.GetComponent<Health>();
                health.Damage();
            }
        }
    }
}