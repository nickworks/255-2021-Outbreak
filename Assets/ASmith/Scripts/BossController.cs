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

        /// <summary>
        /// State Machine for the Boss
        /// </summary>
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

        /// <summary>
        /// Variable containing the player's current location
        /// </summary>
        public Transform playerLocation;

        #region List of Gun Barrels on Boss
        public Transform barrel1;
        public Transform barrel2;
        public Transform barrel3;
        public Transform barrel4;
        public Transform barrel5;
        public Transform barrel6;
        public Transform barrel7;
        public Transform barrel8;
        #endregion

        /// <summary>
        /// Variable containing the Boss gameObject
        /// </summary>
        public GameObject TripleB;

        /// <summary>
        /// Variable containing the turret gameObject
        /// </summary>
        public GameObject turret;

        /// <summary>
        /// Variable containing the pillar gameObject
        /// </summary>
        public GameObject pillar;

        /// <summary>
        /// Variable containing a reference to the BadBullet prefab
        /// </summary>
        public BadBullet prefabBadBullet;

        /// <summary>
        /// Variable containing a reference to the scene's Navigation Mesh
        /// </summary>
        private NavMeshAgent nav;

        /// <summary>
        /// Variable that tracks how long till another ability can be used
        /// </summary>
        private float abilityTimer;

        /// <summary>
        /// Variable that tracks the boss' current health 
        /// </summary>
        private float currHealth;

        /// <summary>
        /// Variable that tracks whether or not the boss can enter rage state
        /// </summary>
        private float rageTrigger = 350;

        /// <summary>
        /// Variable that tracks when the boss can shoot again
        /// </summary>
        private float cooldownShoot = 0;

        /// <summary>
        /// Variable that tracks how fast the boss can shoot
        /// </summary>
        private float roundsPerSecond = 7;

        /// <summary>
        /// Variable that tracks the current amount of rounds in the boss' clip
        /// </summary>
        private int roundsInClip;

        /// <summary>
        /// Variable that tracks that maximum possible rounds in the boss' clip
        /// </summary>
        private int roundsInClipMax = 20;

        /// <summary>
        /// Variable that tracks whether or not the boss battle has begun
        /// </summary>
        public static bool battleBegun = false;

        /// <summary>
        /// Variable that tracks whether or not the boss is raging
        /// </summary>
        private bool isRaging = false;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>(); // Sets the reference to the levels nav mesh
            roundsInClip = roundsInClipMax; // sets the current rounds in the boss' clip to the max
        }

        void Update()
        {
            EnemyHealth health = TripleB.GetComponentInParent<EnemyHealth>(); // Gets a reference to the EnemyHealth class for access to the health variable
            currHealth = health.health; // Tracks the boss' current health in the Enemy State script is makes it accessible here

            if (cooldownShoot > 0) // If cooldown is GREATER THAN 0...
            {
                cooldownShoot -= Time.deltaTime; // count down the cooldown timer
            }

            switch (currentBossState) // State Switcher
            {
                case BossState.Idling: // Idle State: No Actions
                    // Transitions
                    if (battleBegun) { currentBossState = BossState.Attacking; } // If player triggers battleBegun box, switch to attacking state

                    break;                

                case BossState.Attacking: // Attack State: Tracking and Shooting at Player
                    // Behavior
                    FollowPlayer();
                    ShootPlayer();

                    // Transitions
                    if (currHealth <= rageTrigger) { currentBossState = BossState.Raging; } // If current health is LESS THAN or EQUAL TO the rage trigger, switch to raging state
                    break;

                case BossState.SummoningPillar: // Summoning Pillar State: Summons Pillar(s) to obstruct players movement and line of sight
                    // Behavior


                    // Transitions


                    break;

                case BossState.SummoningTurrets: // Summoning Turret State: Summons Turret(s) to attack player
                    // Behavior


                    // Transitions


                    break;

                case BossState.Spinshotting: // Spinshotting State: Boss begins spinning rapidly while shooting at a higher rate of fire
                    // Behavior
                    FollowPlayer();

                    // Transitions


                    break;

                case BossState.Raging: // Raging State: Boss enters an infinite Spinshotting state after reaching a health threshold
                    // Behavior
                    if (!isRaging) // If NOT raging...
                    {
                        SoundBoard.PlayBossRageMusic(); // Begin playing boss rage music
                        isRaging = true; // tells game the boss is in rage mode
                        roundsPerSecond = 12; // Increases rate of fire
                        roundsInClipMax = 50; // Increases max ammo in clip
                    }

                    FollowPlayer();
                    ShootPlayer();

                    break;
            }
        }

        private void ShootPlayer() // Method called when the boss fires its' guns
        {
            if (cooldownShoot > 0) return; // If weapon on cooldown, return
            if (roundsInClip <= 0) // If no ammo in clip, reload
            {
                cooldownShoot = 3; // sets cooldown timer to simulate reloading
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

        private void FollowPlayer() // Method called when the boss must chase the player
        {
            if (playerLocation != null) nav.SetDestination(playerLocation.position); // Sets the boss' destination to the player's current position
        }
    }
}