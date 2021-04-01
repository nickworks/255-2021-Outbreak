using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class BossStates : MonoBehaviour
    {
        static class States
        {
            public class State
            {
                protected BossStates boss;

                virtual public State Update()
                {
                    return null;
                }

                virtual public void OnStart(BossStates boss)
                {
                    this.boss = boss;
                }

                virtual public void OnEnd()
                {

                }
            }

            ////////////////// Children of the State class

            public class Idle : State
            {
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
            public class ClimbWall : State
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
            public class Attack4 : State
            {
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
        }

        // Hold reference to the list of states
        private States.State state;

        // Set pawn to CharacterController
        private CharacterController pawn;

        // Reference the target to move towards
        public Transform attackTarget;

        /// <summary>
        /// This variable is how quickly the boss will move through the environment
        /// </summary>
        public float moveSpeed = 4;

        public Vector3 stepLength = Vector3.one;

        public Vector3 moveDir { get; private set; }

        void Start()
        {
            // Set pawn to get the CharacterController Component
            pawn = GetComponent<CharacterController>();
        }


        void Update()
        {
            // If we are not in a state, use the Idle state
            if (state == null) SwitchState(new States.Idle());

            // If we are in a state, look for the switch condition in the current state's update
            if (state != null) SwitchState(state.Update());
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return; // Can't switch to nothing

            // Call the previous state's on end 
            if (state != null) state.OnEnd();

            // Switch to the new state
            state = newState;

            // Call the new state's on start function
            state.OnStart(this);
        }
    }
}

