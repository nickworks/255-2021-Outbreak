using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class PlayerMovement : MonoBehaviour {

        static class States {
            public class State {

                protected PlayerMovement player;

                virtual public State Update() {
                    return null;
                }

                virtual public void OnStart(PlayerMovement player) {
                    this.player = player;
                }

                virtual public void OnEnd() {

                }
            }

            /// Child Classes:

            public class Idle : State {
                public override State Update() {
                    // behavior:
                    player.MoveThePlayer(0);

                    // transitions:
                    if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
                        return new States.Walking();
                    
                    return null;
                }
            }

            public class Walking : State {
                public override State Update() {
                    // Behavior:
                    player.MoveThePlayer(1);

                    // transitions to other states:

                    if (Input.GetButton("Fire3")) return new States.Sprinting();

                    if (Input.GetKeyDown("space")) // transition to dashing
                        return new States.Dashing();

                    if (!Input.GetKey("w") && !Input.GetKey("a") && !Input.GetKey("s") && !Input.GetKey("d"))
                        return new States.Idle();

                    return null;
                }
            }

            public class Sprinting : State {
                public override State Update() {
                    player.MoveThePlayer(2);

                    // transitions to other states:
                    if (!Input.GetButton("Fire3")) return new States.Idle();

                    return null;
                }

            }

            public class Dashing : State {
                public override State Update() {
                    // Behavior:
                    player.DashMove();

                    // transition:
                    player.dashTimer -= Time.deltaTime;
                    if (player.dashTimer <= 0) return new States.Walking();

                        return null;
                }

                public override void OnEnd() {
                    player.dashTimer = .15f;
                }

            }

        }

        private States.State state;

        private Vector3 move = new Vector3();

        public float playerSpeed = 10;

        private CharacterController pawn;

        /// <summary>
        /// How long a dash should take, in seconds
        /// </summary>
        public float dashDuration = 0.25f;

        /// <summary>
        /// How many meters per second to move while dashing.
        /// </summary>
        public float dashSpeed = 60;

        private Vector3 dashDirection;

        /// <summary>
        /// This stores how many seconds are left:
        /// </summary>
        private float dashTimer = 0;

        void Start() {
            pawn = GetComponent<CharacterController>();
        }


        void Update() {

            if (state == null) SwitchingStates(new States.Idle());

            if (state != null) SwitchingStates(state.Update());
        }

        private void SwitchingStates(States.State newState) {
            if (newState == null) return;

            if (state != null) state.OnEnd();

            state = newState;
            state.OnStart(this);
        }

        private void MoveThePlayer(float mult = 1) {
            // Getting player input values of 0 to 1
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            move = Vector3.right * h + Vector3.forward * v;

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
            
            pawn.Move(dashDirection * Time.deltaTime * dashSpeed);
        }
    }
}
