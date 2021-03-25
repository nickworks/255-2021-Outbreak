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

                    
                    return null;
                }
            }

            public class Walking : State {
                public override State Update() {
                    // Behavior:
                    player.MoveThePlayer(1);

                    // transitions to other states:
                    //if (player.transform.position == player.transform.position)
                        //return new States.Idle();

                    if (Input.GetButton("Fire3")) return new States.Sprinting();

                    if (Input.GetButtonDown("Fire2")) // transition to dashing
                        return new States.Dashing();

                    return null;
                }
            }

            public class Sprinting : State {

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

            public class AssualtRife : State {

            }
            public class RifleReload : State {

            }

            public class Rockets : State {

            }

            public class Artilary : State {

            }

        }

        private States.State state;



        public enum MoveState {
            Regular, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking, //3
            Shielding,
        }

        private Vector3 move = new Vector3();

        public float playerSpeed = 10;

        private CharacterController pawn;

        //private float dashTime = .3f;
        MoveState currentMoveState = MoveState.Regular;

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


        // 1 = regular
        // 2 = dashing 
        // 3 = sneaking

        void Start() {
            pawn = GetComponent<CharacterController>();
        }


        void Update() {

            if (state == null) SwitchingStates(new States.Walking());

            if (state != null) SwitchingStates(state.Update());

            //print(currentMoveState);
            /*
            switch (currentMoveState) {
                case MoveState.Regular:

                    // do behaviour for this state:


                    break;
                case MoveState.Dashing:

                    // do behaviour for this state:
                    //DashMove(25);
                    DashThePlayer();

                    

                    // transitions to other states:
                    

                    //dashTime -= Time.deltaTime;
                    //if (dashTime <= 0) {
                    //    currentMoveState = MoveState.Regular;
                    //    dashTime = .3f;
                    //}

                    break;
                case MoveState.Sprinting:

                    // do behaviour for this state:

                    MoveThePlayer(2);

                    // transitions to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;

                    break;
                case MoveState.Sneaking:

                    // do behaviour for this state:

                    MoveThePlayer(.5f);

                    // transitions to other states:
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;

                    break;
            }

*/
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

        //private void DashMove(float dashSpeed = 1f) {
        //    pawn.Move(transform.forward * dashSpeed * Time.deltaTime);
        
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
