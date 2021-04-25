using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class BossTrigger : MonoBehaviour
    {
        /// <summary>
        /// Variable used as a reference to the battleBegun boolean in the BoddController script
        /// Tracks whether or not the boss battle has begun
        /// </summary>
        BossController battleBegun;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player") // If object tagged as "Player" triggers the box...
            {
                if (battleBegun == false) // If battle has NOT begun
                {
                    SoundBoard.PlayBossFightMusic(); // Play Boss fight music
                    BossController.battleBegun = true; // set battle begun to true
                    Destroy(gameObject); // Destroy the trigger box
                }
            }
        }
    }
}
