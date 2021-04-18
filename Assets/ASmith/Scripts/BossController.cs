using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System;

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
        private float cooldownShoot;
        private float roundsPerSecond;

        public static bool battleBegun = false;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

            currHealth = healthMax;
        }

        void Update()
        {
            switch (currentBossState)
            {
                case BossState.Idling:
                    // Transitions
                    if (battleBegun) { currentBossState = BossState.Attacking; }

                    break;                

                case BossState.Attacking:
                    // Behavior
                    FollowPlayer();

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

        private void FollowPlayer()
        {
            if (playerLocation != null) nav.SetDestination(playerLocation.position);
        }
    }
}