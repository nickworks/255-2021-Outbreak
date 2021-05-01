using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Velting
{
    public class EnemyBasicController : MonoBehaviour
    {
        static class States
        {
            public class State
            {
                protected EnemyBasicController enemy;

                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(EnemyBasicController enemy)
                {
                    this.enemy = enemy;
                }
                virtual public void OnEnd()
                {

                }
            }

            //////////////////////// Children of State
            public class Idle : State 
            {
                public override State Update()
                {
                    //behavior:

                    //transition:
                    if (enemy.EnemyCanPursue()) return new States.Pursuing();

                    return null;
                }
            }
            public class Pursuing : State 
            {
                public override State Update()
                {
                    //behavior:
                    if (enemy.attackTarget != null) enemy.nav.SetDestination(enemy.attackTarget.position);
                    //transition:
                    if (!enemy.EnemyCanPursue()) return new States.Idle();
                    if (enemy.health <= 0) return new States.Death();

                    return null;
                }
            
            }
            public class Death: State
            {
                public override State Update()
                {
                    enemy.Death();
                    return null;
                }
            }
           
        }

        private States.State state;

        public NavMeshAgent nav;

        public Transform attackTarget;

        private Vector3 disToPlayer;
        private float pursuitDis = 50;
        public float health = 30;
        
        public void Death()
        {
            Destroy(gameObject);
        }

        void Start()
        {

            nav = GetComponent<NavMeshAgent>();

        }
        void Update()
        {
            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update());
        }
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // Don't switch to nothing...

            // Call the current state's onEnd function.
            if (state != null) state.OnEnd();

            // Switch the state.
            state = newState;

            // Call the new state's onStart function.
            state.OnStart(this);
        }

        private bool EnemyCanPursue()
        {
            if (!attackTarget) return false;

            disToPlayer = attackTarget.transform.position - transform.position;

            if (disToPlayer.sqrMagnitude > pursuitDis) return false;
            if (disToPlayer.sqrMagnitude < pursuitDis) return true;

            return false;

        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player.health > 0) player.health -= 10;
            player.playerHit = true;

            
        }
    }
}
