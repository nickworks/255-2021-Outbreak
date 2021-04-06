using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
                    boss.CanSeePlayer();
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
                    if (boss.canSeePlayer && boss.vToPlayer.sqrMagnitude < boss.attackDis * boss.attackDis) boss.SwitchState(new States.Attack1());
                    return null;
                }
            }
            public class ClimbWall : State
            {
                public override State Update()
                {
                    // Behavior:
                    boss.ClimbWall();
                    // Transitions:
                    return null;
                }
            }
            public class Stunned : State
            {
                public override State Update()
                {
                    // Behavior:
                    boss.Stunned();
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
                    boss.DoAttack1();
                    // Transitions:
                    return null;
                }
            }
            public class Attack2 : State
            {
                public override State Update()
                {
                    // Behavior:
                    boss.DoAttack2();
                    // Transitions:
                    return null;
                }
            }
            public class Attack3 : State
            {
                public override State Update()
                {
                    // Behavior:
                    boss.DoAttack3();
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
        /// Reference the Boss's neck bone in the inspector
        /// </summary>
        public Transform neckBone;

        /// <summary>
        /// Reference the Boss's torso bone in the inspector
        /// </summary>
        public Transform TorsoBone;

        /// <summary>
        /// Reference the boss's left shoulder in the inspector
        /// </summary>
        public Transform shoulderLeft;

        /// <summary>
        /// Reference the Boss's Right shoulder in the inspector
        /// </summary>
        public Transform shoulderRight;

        /// <summary>
        /// Reference the Boss's left hip in the inspector
        /// </summary>
        public Transform hipLeft;

        /// <summary>
        /// Reference the Boss's right hip in the inspector
        /// </summary>
        public Transform hipRight;

        /// <summary>
        /// Reference the boss's tail bone in the inspector
        /// </summary>
        public Transform tailBone;

        private Vector3 vToPlayer;

        private float moveSpeed = .5f;

        /// <summary>
        /// How far can the boss see
        /// </summary>
        public float visDis = 10;

        public float attackDis = 6;

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

            // Pass the result of the canseeplayer function into a boolean.
            canSeePlayer = CanSeePlayer();
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
        private bool CanSeePlayer()
        {
            if (!attackTarget) return false;

            vToPlayer = attackTarget.transform.position - transform.position;

            if (vToPlayer.sqrMagnitude > visDis * visDis) return false;
            if (Vector3.Angle(transform.forward, vToPlayer) > visCone) return false;

            if (vToPlayer.sqrMagnitude < visDis * visDis && Vector3.Angle(transform.forward, vToPlayer) < visCone) return true;

            return false;
        }

        void MoveTheBoss()
        {
            if (!attackTarget) return;

            // Turn the boss to look at the player
            Quaternion rotateToPlayer = Quaternion.LookRotation(vToPlayer, Vector3.up);

            // Get local Euler angles before rotation
            Vector3 euler1 = transform.localEulerAngles;
            Quaternion previousRot = transform.rotation;

            // Do rotation
            transform.rotation = rotateToPlayer;

            // Get new local euler angles 
            float eulerY = transform.localEulerAngles.y;
            float eulerZ = transform.localEulerAngles.z;

            // Revert the rotation
            transform.rotation = previousRot;

            // Ease the rotation to the target.
            transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(0, eulerY, eulerZ), .3f);

            // Move in the direction of the player
            pawn.SimpleMove(vToPlayer * moveSpeed);
        }

        void DoAttack1()
        {
            Quaternion startingLeftArmRot = shoulderLeft.transform.localRotation;

            Quaternion targetLeftArmRot = startingLeftArmRot * Quaternion.Euler(50, 0, 10);

            shoulderLeft.transform.localRotation = AnimMath.Slide(startingLeftArmRot, targetLeftArmRot, .01f);
        }
        void DoAttack2()
        {

        }
        void DoAttack3()
        {

        }       
        void Stunned()
        {
            Quaternion startingNeckRot = neckBone.transform.localRotation;
            Quaternion startingLeftArmRot = shoulderLeft.transform.localRotation;
            Quaternion startingRightArmRot = shoulderRight.transform.localRotation;

            Quaternion targetNeckRot = startingNeckRot * Quaternion.Euler(50, 0, 0);

        }
        void ClimbWall()
        {

        }
    }
}

