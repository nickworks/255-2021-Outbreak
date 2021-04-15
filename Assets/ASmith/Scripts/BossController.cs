using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace ASmith
{
    public class BossController : MonoBehaviour
    {
        public static BossController main;

        public enum BossState
        {
            Idling, // 1
            Attacking, // 2
            SummoningPillar, // 3 
            SummoningTurrets, // 4
            Spinshotting, // 5
            Raging // 6
        }

        BossState currentBossState = BossState.Idling;

        #region Unfinished Random Number Picker
        // This section was going to be used to pick from a random set of numbers to
        // be the fire rate for the boss. I have no clue how to make it work at the moment

        //public int[] rpsRange = new int[5] { 3, 4, 5, 6, 7 };

        //public int randomRange = UnityEngine.Random.Range(0, rpsRange.Length);
        //int randomNumber = rpsRange[randomRange];
        #endregion

        public Transform playerLocation;

        public Transform barrel1;
        public Transform barrel2;
        public Transform barrel3;
        public Transform barrel4;
        public Transform barrel5;
        public Transform barrel6;
        public Transform barrel7;
        public Transform barrel8;

        public GameObject turret;
        public GameObject pillar;

        public BadBullet prefabBadBullet;

        private NavMeshAgent nav;

        private float abilityTimer;
        private float currHealth;
        private float healthMin;
        private float healthMax;
        private float rageTrigger;
        private float cooldownShoot = 0;
        // TODO: Randomize roundsPerSecond with above Random Number Picker
        private float roundsPerSecond;

        private int roundsInClip;
        private int roundsInClipMax = 7;

        public static bool battleBegun = false;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

            currHealth = healthMax;
            roundsInClip = roundsInClipMax;
        }

        void Update()
        {
            if (cooldownShoot > 0)
            {
                cooldownShoot -= Time.deltaTime; // counts down the cooldown timer
            }

            switch (currentBossState)
            {
                case BossState.Idling:
                    // Transitions
                    if (battleBegun) { currentBossState = BossState.Attacking; }

                    break;                

                case BossState.Attacking:
                    // Behavior
                    FollowPlayer();
                    ShootPlayer();

                    // Transitions


                    break;

                case BossState.SummoningPillar:
                    // Behavior


                    // Transitions


                    break;

                case BossState.SummoningTurrets:
                    // Behavior


                    // Transitions


                    break;

                case BossState.Spinshotting:
                    // Behavior
                    FollowPlayer();

                    // Transitions


                    break;

                case BossState.Raging:
                    // Behavior
                    FollowPlayer();

                    // Transitions


                    break;
            }
        }

        private void ShootPlayer()
        {
            if (cooldownShoot > 0) return; // If weapon on cooldown, return
            if (roundsInClip <= 0) // If no ammo in clip, reload
            {
                cooldownShoot = 4; // sets cooldown timer to simulate reloading
                roundsInClip = roundsInClipMax; // sets roundsInClip back to max
                return; // returns to beginning of loop
            }

            #region Bullet Instantiation and Launch Logic
            BadBullet p1 = Instantiate(prefabBadBullet, barrel1.transform.position, barrel1.rotation);
            BadBullet p2 = Instantiate(prefabBadBullet, barrel2.transform.position, barrel2.rotation);
            BadBullet p3 = Instantiate(prefabBadBullet, barrel3.transform.position, barrel3.rotation);
            BadBullet p4 = Instantiate(prefabBadBullet, barrel4.transform.position, barrel4.rotation);
            BadBullet p5 = Instantiate(prefabBadBullet, barrel5.transform.position, barrel5.rotation);
            BadBullet p6 = Instantiate(prefabBadBullet, barrel6.transform.position, barrel6.rotation);
            BadBullet p7 = Instantiate(prefabBadBullet, barrel7.transform.position, barrel7.rotation);
            BadBullet p8 = Instantiate(prefabBadBullet, barrel8.transform.position, barrel8.rotation);

            p1.InitBullet(barrel1.transform.forward * 10);
            p2.InitBullet(barrel2.transform.forward * 10);
            p3.InitBullet(barrel3.transform.forward * 10);
            p4.InitBullet(barrel4.transform.forward * 10);
            p5.InitBullet(barrel5.transform.forward * 10);
            p6.InitBullet(barrel6.transform.forward * 10);
            p7.InitBullet(barrel7.transform.forward * 10);
            p8.InitBullet(barrel8.transform.forward * 10);
            #endregion

            roundsInClip--; // Subtracts one bullet from clip after firing
            cooldownShoot = 1 / roundsPerSecond; // restarts weapon cooldown
        }

        private void FollowPlayer()
        {
            if (playerLocation != null) nav.SetDestination(playerLocation.position);
        }
    }
}