using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class controls game rules
    /// </summary>
    public class GameFunctions : MonoBehaviour {

        /// <summary>
        /// Getting boss transform to see if it exist or not
        /// </summary>
        public Transform boss;

        /// <summary>
        /// Getting boss transform to see if it exist or not
        /// </summary>
        public Transform player;

        /// <summary>
        /// if the game is paused or not
        /// </summary>
        //bool pauseGame = true;

        void Update() {

            if (!boss) Outbreak.Game.GotoNextLevel(); // if boss is dead, then go to the next level

            if (!player) Outbreak.Game.GameOver(); // if player is dead, then game over
/*
            if (Input.GetKeyDown("p")) {
                if (pauseGame) {
                    Time.timeScale = 0;
                    pauseGame = false;
                } else {
                    Time.timeScale = 1;
                    pauseGame = true;
                }
            }
*/
        }
    }
}