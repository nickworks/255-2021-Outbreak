using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
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

        public Vector3 dashDirection;

        public float dashSpeed = 100;

        public float dashDuration = 0.25f;

        /// <summary>
        /// How many seconds are left:
        /// </summary>
        private float dashTimer = 0;

        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regular;

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        void Update()
        {
            print(currentMoveState);

            switch (currentMoveState)
            {
                case MoveState.Regular:

                    // Do behavior for this state
                    MovePlayer(1);

                    // Transition to other states
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    //else if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButtonDown("Fire2"))
                    {
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxisRaw("Horizontal"); // Gives -1, 0, or 1
                        float v = Input.GetAxisRaw("Vertical"); // Gives -1, 0, or 1
                        dashDirection = new Vector3(h, 0, v).normalized; //returns a normalized version of this vector.
                        //dashDirection.Normalize();
                        dashTimer = .15f;

                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize();
                    }

                    break;
                case MoveState.Dashing:

                    // Do behavior for this state
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;

                    // Transition to other states
                    if (dashTimer <= 0) currentMoveState = MoveState.Regular;
                    break;
                case MoveState.Sprinting:

                    // Do behavior for this state
                    MovePlayer(2);

                    // Transition to other states
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    //else if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    break;
                case MoveState.Sneaking:

                    // Do behavior for this state
                    MovePlayer(.5f);

                    // Transition to other states
                    //if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;
                    break;
            }

        }

        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * dashSpeed);
        }

        private void MovePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagonal input vectors

            pawn.Move(move * Time.deltaTime * playerSpeed * mult);
        }
    }
}

