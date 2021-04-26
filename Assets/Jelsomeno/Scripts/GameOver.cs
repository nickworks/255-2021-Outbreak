using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
{
    public class GameOver : MonoBehaviour
    {

        public Transform boss;

        public Transform player;


        // Update is called once per frame
        void Update()
        {

            if (!boss) Outbreak.Game.GotoNextLevel(); // if boss is dead, then go to the next level

            if (!player) Outbreak.Game.GameOver(); // if player is dead, then game over

        }
    }
}
