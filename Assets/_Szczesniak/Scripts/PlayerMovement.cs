using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class uses state pattern to move the player
    /// </summary>
    public class PlayerMovement : MonoBehaviour {

        /// <summary>
        /// The state pattern class
        /// </summary>
        static class States {
            public class State {

                /// <summary>
                /// To get access outside of this child class, boss is needed to access outside variables.
                /// </summary>
                protected PlayerMovement player;

                /// <summary>
                /// Sets update up.
                /// </summary>
                /// <returns></returns>
                virtual public State Update() {
                    return null;
                }

                /// <summary>
                /// Referencing PlayerMovement
                /// </summary>
                /// <param name="player"></param>
                virtual public void OnStart(PlayerMovement player) {
                    this.player = player;
                }

                /// <summary>
                /// Tell when it is done
                /// </summary>
                virtual public void OnEnd() {

                }
            }

            //////// Child Classes:
            
            /// <summary>
            /// When the player is not moving 
            /// </summary>
            public class Idle : State {
                public override State Update() {
                    // behavior:
                    player.MoveThePlayer(0); // makes player stop moving

                    // transitions:
                    if (player.playerHealth.health <= 0) return null; // if player is dead, stops everything

                    if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) // checks if player is pressing any of the movement keys
                        return new States.Walking(); // goes to Walking() state
                    
                    return null;
                }
            }

            /// <summary>
            /// This state makes the player walk/move
            /// </summary>
            public class Walking : State {
                public override State Update() {
                    // Behavior:
                    player.MoveThePlayer(1); // makes the player move from sending the paramter 1

                    // transitions to other states:
                    if (player.playerHealth.health <= 0) return new States.Idle(); // if player is dead, goes to Idle() state

                    if (Input.GetButton("Fire3")) return new States.Sprinting(); // if the player press sprint, goes to Sprinting() state

                    if (Input.GetKeyDown("space") && player.dashTimeToUseAgain <= 0) { // Transition to Dashing when player presses space bar
                        SoundEffectBoard.DashSound(); // plays dash sound effect
                        player.dashTrail.Play();
                        return new States.Dashing(); // goes to Dashing() state
                    }

                    if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d")) // if the player is not pressing anything
                        return new States.Idle(); // goes to Idle() state

                    return null;
                }
            }

            /// <summary>
            /// This state makes the player sprint
            /// </summary>
            public class Sprinting : State {
                public override State Update() {
                    player.MoveThePlayer(2); // sending a parameter of 2 makes the player run

                    // transitions to other states:
                    if (player.playerHealth.health <= 0) return new States.Idle(); // if player health is equal 0 or less, goes to Idle() state

                    if (!Input.GetButton("Fire3")) return new States.Idle(); // if the player is not pressing shift, goes to Idle() state

                    return null;
                }

            }

            /// <summary>
            ///  This state makes the player dash
            /// </summary>
            public class Dashing : State {
                public override State Update() {
                    // Behavior:
                    player.DashMove(); // makes player dash

                    // transition:
                    if (player.playerHealth.health <= 0) return new States.Idle(); // if player health is equal 0 or less, goes to Idle() state


                    player.dashTimer -= Time.deltaTime; // count dashTimer down
                    if (player.dashTimer <= 0) return new States.Walking(); // goes back to Walking() state when dashTime is at 0 or less

                        return null;
                }

                public override void OnEnd() {
                    player.dashTimer = .15f; // resets dashTimer
                }

            }

        }

        /// <summary>
        /// access the state pattern, maintain it, and make it function.
        /// </summary>
        private States.State state;

        /// <summary>
        /// Vector to move the player
        /// </summary>
        private Vector3 move = new Vector3();

        /// <summary>
        /// Base speed of the player.
        /// </summary>
        public float playerSpeed = 10;

        /// <summary>
        /// The controller of the player
        /// </summary>
        private CharacterController pawn;

        /// <summary>
        /// How long a dash should take, in seconds
        /// </summary>
        public float dashDuration = 0.25f;

        /// <summary>
        /// How many meters per second to move while dashing.
        /// </summary>
        public float dashSpeed = 60;

        /// <summary>
        /// Direction to dash at
        /// </summary>
        private Vector3 dashDirection;

        /// <summary>
        /// This stores how many seconds are left:
        /// </summary>
        private float dashTimer = .15f;

        /// <summary>
        /// When to be able to use dash again
        /// </summary>
        [HideInInspector] public float dashTimeToUseAgain = 0;

        /// <summary>
        /// Used to reset the dash time
        /// </summary>
        public float dashTimerToUseAgainSetter = 2f;

        /// <summary>
        /// Gets player's health
        /// </summary>
        private HealthScript playerHealth;

        private ParticleSystem dashTrail;

        void Start() {
            pawn = GetComponent<CharacterController>(); // Gets CharacterController
            playerHealth = GetComponent<HealthScript>(); // Gets HealthScript
            dashTrail = GetComponentInChildren<ParticleSystem>();
        }


        void Update() {

            // if nothing is assigned to the state, then make the state go to the Regular() state
            if (state == null) SwitchingStates(new States.Idle());

            if (state != null) SwitchingStates(state.Update()); // makes the state run it's update method

            if (dashTimeToUseAgain > 0) dashTimeToUseAgain -= Time.deltaTime; // Timer to be able to use dash again
        }

        /// <summary>
        /// Makes the state swtich to a different state
        /// </summary>
        /// <param name="newState"></param>
        private void SwitchingStates(States.State newState) {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done

            state = newState; // swap states
            state.OnStart(this);
        }

        /// <summary>
        /// Moves the player 
        /// </summary>
        /// <param name="mult"></param>
        private void MoveThePlayer(float mult = 1) {
            // Getting player input values of 0 to 1
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            move = Vector3.right * h + Vector3.forward * v; // determines if the player moves or not

            //move.Normalize(); Doesn't work with gamepads like this also expensive

            if (move.sqrMagnitude > 1) move.Normalize(); // fix bug with diagonal input vectors

            //pawn.Move(move * Time.deltaTime * playerSpeed * mult);
            pawn.SimpleMove(move * playerSpeed * mult);
        }
        
        private void DashMove() {
            float h = Input.GetAxisRaw("Horizontal"); // either -1, 0, 1
            float v = Input.GetAxisRaw("Vertical");

            dashDirection = new Vector3(h, 0, v).normalized;
            //dashDirection.Normalize();

            // clamp the length of dashDir to 1:
            if (dashDirection.sqrMagnitude > 1)
                dashDirection.Normalize();
            
            pawn.Move(dashDirection * Time.deltaTime * dashSpeed); // move the player in a dash

            dashTimeToUseAgain = dashTimerToUseAgainSetter; // sets the dash timer
        }
    }
}
