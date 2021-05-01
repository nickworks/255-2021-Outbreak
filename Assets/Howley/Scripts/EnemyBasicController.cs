using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Howley
{
    public class EnemyBasicController : MonoBehaviour
    {
        /// <summary>
        /// Set up the state design pattern
        /// </summary>
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
            public class Idle : State {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
            public class Persuing : State 
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
            public class Patrolling : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
            public class Stunned : State 
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
            public class Death : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
            public class Attack1 : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
            public class Attack2 : State 
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
            public class Attack3 : State 
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
        }

        /// <summary>
        /// Hold refence to the states class
        /// </summary>
        private States.State state;

        /// <summary>
        /// Hold reference to the navmesh in the scene
        /// </summary>
        private NavMeshAgent nav;

        /// <summary>
        /// Hold reference to the target for the enemy
        /// </summary>
        public Transform attackTarget;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

        }

        /// <summary>
        /// Update is called every game tick
        /// </summary>
        void Update()
        {
            if (attackTarget != null) nav.SetDestination(attackTarget.position);

            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update());
        }

        /// <summary>
        /// This function is called when the enemy is trying to switch states
        /// </summary>
        /// <param name="newState"></param>
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
    }
}

