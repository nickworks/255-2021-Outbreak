using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hodgkins
{
    public class BossController : MonoBehaviour
    {

        static class States
        {
            public class State
            {
                protected BossController boss;

                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(BossController boss)
                {
                    this.boss = boss;
                }
                virtual public void OnEnd()
                {

                }
            }

            public class Idle : State { }
            public class Pursuing : State { }
            public class Death : State { }
            public class Attack1 : State { }
            public class Attack2 : State { }
            public class Attack3 : State { }
        }

        private States.State state;

        private NavMeshAgent nav;

        public Transform attackTarget;

        public Transform basicEnemy;

        private float spawnCooldown = 0;


        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

        }

        // Update is called once per frame
        void Update()
        {
            if (attackTarget != null) nav.SetDestination(attackTarget.position);

            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update());

            if (spawnCooldown > 0) spawnCooldown -= Time.deltaTime;

            SpawnBasicEnemy();

            //RandomAttack();

        }
        
          private void SpawnBasicEnemy()
          {
            if (spawnCooldown > 0) return; // wait longer to spawn again...
        
            Transform e = Instantiate(basicEnemy, transform.position, Quaternion.identity);
        
            spawnCooldown = 10;
          }
        
        private void RandomAttack()
        {
            int attackNum = 0; // Will implement random selector when I figure out how to do that


            switch(attackNum)
            {
                case 0:

                    break;

                case 1:

                    break;

                case 2:

                    break;
            }
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);


        }
    }
}