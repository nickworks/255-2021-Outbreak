using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class GameFunctions : MonoBehaviour {

        public Transform boss;
        public Transform player;
        bool pauseGame = true;

        void Update() {

            if (!boss) Outbreak.Game.GotoNextLevel();

            if (!player) Outbreak.Game.GameOver();

            if (Input.GetKeyDown("p")) {
                if (pauseGame) {
                    Time.timeScale = 0;
                    pauseGame = false;
                } else {
                    Time.timeScale = 1;
                    pauseGame = true;
                }
            }

        }
    }
}