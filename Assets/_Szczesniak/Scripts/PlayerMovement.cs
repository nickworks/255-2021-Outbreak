using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class PlayerMovement : MonoBehaviour {

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

        private float dashTime = .3f;
        MoveState currentMoveState = MoveState.Regular;

        /// <summary>
        /// How long a dash should take, in seconds
        /// </summary>
        public float dashDuration = 0.25f;

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

            //print(currentMoveState);

            switch (currentMoveState) {
                case MoveState.Regular:

                    // do behaviour for this state:

                    MoveThePlayer(1);

                    // transitions to other states:
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButtonDown("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButtonDown("Fire2")) { // transition to dashing
                       
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxis("Horizontal");
                        float v = Input.GetAxis("Vertical");
                        dashDirection = new Vector3(h, 0, v);

                        // clamp the length of dashDir to 1:
                        if (dashDirection.sqrMagnitude > 1)
                            dashDirection.Normalize();
                    }

                    break;
                case MoveState.Dashing:

                    // do behaviour for this state:
                    //DashMove(25);
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;

                    // transitions to other states:

                    if (dashTimer <= 0) currentMoveState = MoveState.Regular;

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
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;

                    break;
                case MoveState.Sneaking:

                    // do behaviour for this state:

                    MoveThePlayer(.5f);

                    // transitions to other states:
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;

                    break;
            }


        }

        private void DashThePlayer() {
            pawn.Move(dashDirection * Time.deltaTime * 100);
        }

        private void MoveThePlayer(float mult = 1) {
            // Getting player input values of 0 to 1
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            move = Vector3.right * h + Vector3.forward * v;

            //move.Normalize(); Doesn't work with gamepads like this also expensive

            if (move.sqrMagnitude > 1) move.Normalize(); // fix bug with diagonal input vectors

            pawn.Move(move * Time.deltaTime * playerSpeed * mult);
        }

        //private void DashMove(float dashSpeed = 1f) {
        //    pawn.Move(transform.forward * dashSpeed * Time.deltaTime);
        //}
    }
}