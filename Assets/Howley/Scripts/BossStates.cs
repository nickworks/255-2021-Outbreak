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
                    // TODO: Make Idle animation
                    // Transitions:
                    if (boss.canSeePlayer) boss.SwitchState(new States.Persuing());
                    return null;
                }
            }
            public class Persuing : State
            {
                public override State Update()
                {
                    // Behavior:
                    boss.MoveTheBoss();
                    // Transitions:
                    if (!boss.canSeePlayer) boss.SwitchState(new States.Idle());
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

        private Vector3 vToPlayer;

        private float moveSpeed = 5;

        /// <summary>
        /// How far can the boss see
        /// </summary>
        public float visDis = 10;

        /// <summary>
        /// The ange at which the boss can see 
        /// </summary>
        public float visCone = 160;

        /// <summary>
        /// Depending on the vision distance, and cone.
        /// </summary>
        private bool canSeePlayer = false;


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

        void MoveTheBoss()
        {
            if (attackTarget)
            {
                vToPlayer = attackTarget.transform.position - transform.position;

                if (vToPlayer.sqrMagnitude > visDis * visDis) canSeePlayer = false;
                if (Vector3.Angle(transform.forward, vToPlayer) > visCone) canSeePlayer = false;

                if (vToPlayer.sqrMagnitude < visDis * visDis && Vector3.Angle(transform.forward, vToPlayer) < visCone) canSeePlayer = true;

                if (canSeePlayer) pawn.SimpleMove(vToPlayer * moveSpeed);              
            }
        }
    }
}

