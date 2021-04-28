using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
{
    /// <summary>
    /// this class will go to the game over screen once one of the player or boss dies
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        /// <summary>
        /// Getting the reference to the boss
        /// </summary>
        public Transform boss;

        /// <summary>
        /// Getting the reference to the player
        /// </summary>
        public Transform player;


        // Update is called once per frame
        void Update()
        {

            if (!boss) Outbreak.Game.GameOver(); // if boss is dead, then the game is over

            if (!player) Outbreak.Game.GameOver(); // if player is dead, then the is game over

        }
    }
}
