using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ASmith
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

            /////////////////////////// Child Classes:

            public class Idle : State { }
            public class Pursuing : State { }
            public class Patrolling : State { }
            public class Stunned : State { }
            public class Death : State { }
            public class Attack1 : State { }
            public class Attack2 : State { }
            public class Attack3 : State { }

        }

        private States.State state;

        private NavMeshAgent nav;

        /// <summary>
        /// Tracks the chosen target for the enemy to attack
        /// </summary>
        public Transform attackTarget;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();            
        }

        void Update()
        {
            if (attackTarget != null) nav.SetDestination(attackTarget.position);  // If there is a target, get target position

            if (state == null) SwitchState(new States.Idle()); // If no target, become idle

            if (state != null) SwitchState(state.Update());
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return; // dont switch to nothing...
            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap state
            state.OnStart(this);
        }
    }
}
