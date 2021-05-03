using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Velting
{
    public class WinLossManager : MonoBehaviour
    {

        public bool isPlayerDead;
        public bool isBossDead;

        public float lossTimer = 3;
        public float winTimer = 3;

        void Start()
        {
            
        }


        void Update()
        {
            if (isBossDead) winTimer -= Time.deltaTime;
            if (isPlayerDead) lossTimer -= Time.deltaTime;
            if (winTimer <= 0) Outbreak.Game.GotoNextLevel();
            if (lossTimer <= 0) Outbreak.Game.GameOver();
        }
    }
}