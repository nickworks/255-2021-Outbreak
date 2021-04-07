using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins {
    public class PlayerMovement : MonoBehaviour
    {
        public enum MoveState
        {
            Regular, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking // 3
        }
                        
        public float playerSpeed = 10;
        private CharacterController pawn;
        MoveState currentMoveState = MoveState.Regular;

        private Vector3 dashDirection;
        public float dashSpeed = 50;
        /// <summary>
        /// This stores how many seconds are left in dash
        /// </summary>
        private float dashTimer = 0;
        /// <summary>
        /// How long a dash lasts, in seconds
        /// </summary>
        public float dashDuration = 0.25f;

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            switch (currentMoveState)
            {
                case MoveState.Regular:
                    // do behaviour for this state
                    MoveThePlayer(1);
                    // transition to other state
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButtonDown("Fire2"))  // transition to dashing
                    {
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxisRaw("Horizontal"); // -1 or 0 or 1
                        float v = Input.GetAxisRaw("Vertical"); // -1 or 0 or 1
                        dashDirection = new Vector3(h, 0, v);
                        dashDirection.Normalize();
                        dashTimer = .25f;

                        // clamp the length of DashDir to 1
                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize();
                    }
                    break;
                case MoveState.Dashing:
                    // do behaviour for this state
                    DashThePlayer();
                    dashTimer -= Time.deltaTime;
                    // transition to other state
                    if (dashTimer <= 0) currentMoveState = MoveState.Regular;
                    break;
                case MoveState.Sprinting:
                    // do behaviour for this state
                    MoveThePlayer(2);
                    // transition to other state
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    break;
                case MoveState.Sneaking:
                    // do behaviour for this state
                    MoveThePlayer(0.5f);
                    // transition to other state
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;

                    break;
            }



        }
        /// <summary>
        /// This function moes the player while they are dashing;
        /// </summary>
        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * 80);
        }
        
        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix diagonal movement bug

            //pawn.Move(move * Time.deltaTime * playerSpeed * mult);
            pawn.SimpleMove(move * playerSpeed * mult);
        }
    }
}