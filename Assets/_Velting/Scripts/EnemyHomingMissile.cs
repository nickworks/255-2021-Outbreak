using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Velting
{
    public class EnemyHomingMissile : MonoBehaviour
    {
        /// <summary>
        /// Current direction/speed
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// How long until the projectile is destroyed.
        /// </summary>
        private float lifeSpan = 5;

        /// <summary>
        /// How long the projectile has lived.
        /// </summary>
        private float age = 0;

        public NavMeshAgent nav;

        public Transform attackTarget;




        void Start()
        {

            nav = GetComponent<NavMeshAgent>();
        }
        

        void Update()
        {
            age += Time.deltaTime;
            if (age > lifeSpan)
            {
                Destroy(gameObject);
            }

            // Euler physics
            nav.SetDestination(attackTarget.transform.position);

            AimAtPlayer();



        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            //EnemyBossController boss = other.GetComponent<EnemyBossController>();

            if (player.health > 0) player.health -= 30;
            // if (boss.health > 0) boss.health -= 10;



            Destroy(gameObject);

        }

        public void BulletGone()
        {
            Destroy(gameObject);
        }

        public void AimAtPlayer()
        {


            Vector3 hitPos = attackTarget.transform.position;


            Vector3 vectorToHitPos = hitPos - transform.position;


            float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

            angle /= Mathf.PI; //convert from "radians" to "half-circles"
            angle *= 180; //convert from "half-circles" to "degrees"

            transform.eulerAngles = new Vector3(0, angle, 0);

        }

    }
}
