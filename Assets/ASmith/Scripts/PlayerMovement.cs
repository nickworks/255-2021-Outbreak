using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum MoveState
        {
            Regular, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking, // 3
            Shielding, // 4
        }

        public float playerSpeed = 10;

        /// <summary>
        /// How long a dash should take in seconds
        /// </summary>
        public float dashDuration = .25f;

        /// <summary>
        /// Stores how many seconds left in dash
        /// </summary>
        private float dashTimer = 0;

        /// <summary>
        /// How strong the dash is
        /// The higher the number, the stronger the dash
        /// </summary>
        public float dashSpeed = 50;

        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regular;

        private Vector3 dashDirection;

        

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        void Update()
        {
            //print(currentMoveState);

            switch (currentMoveState)
            {
                case MoveState.Regular:

                    // Do behavior for this state:
                    MoveThePlayer(1);

                    // Transition to other states:
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting; // When holding shift start sprinting state
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking; // When holding ctrl go to sneak state
                    if (Input.GetButtonDown("Fire2")) // On right mouse down, go to dash state
                    {
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxisRaw("Horizontal"); // Raw makes these number default to wholes rather than decimals: -1/0/1
                        float v = Input.GetAxisRaw("Vertical"); // Raw makes these number default to wholes rather than decimals: -1/0/1
                        dashDirection = new Vector3(h, 0, v); // Ties the dash vector to "h" and "v" for the x and z axis respectively
                        dashDirection.Normalize();
                        dashTimer = .25f;

                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize(); // Clamps the length of dash to 1 so diagonal movement is same length
                    }
                    break;
                case MoveState.Dashing:

                    // Do behavior for this state:
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;
                    // Transition to other states:
                    if (dashTimer <= 0) currentMoveState = MoveState.Regular;

                    break;
                case MoveState.Sprinting:

                    // Do behavior for this state:
                    MoveThePlayer(2);

                    // Transition to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular; // When not holding shift return to regular state
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Regular; // When holding ctrl go to sneak state

                    break;
                case MoveState.Sneaking:

                    // Do behavior for this state:
                    MoveThePlayer(.5f);

                    // Transition to other states:
                    //if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular; // When not holding ctrl return to regular state

                    break;
            }
        }

        /// <summary>
        /// This function moves the player while they are dashing
        /// </summary>
        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * dashSpeed); 
        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v; // Calculates what direction to walk in

            if (move.sqrMagnitude > 1) move.Normalize(); // Fixes bug that makes diagonal movement faster

            pawn.Move(move * Time.deltaTime * playerSpeed * mult); // Moves the player based on created variables
        }
    }
}

