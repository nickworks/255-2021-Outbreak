using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// Set up a state machine for the player to use
        /// </summary>
        public enum MoveState
        {
            Regular, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking // 3
        }

        /// <summary>
        /// This value is multiplied to a vector to give the player quicker movement
        /// </summary>
        public float playerSpeed = 10;

        /// <summary>
        /// This variable is used in unison to the mouse position to dash in a direction
        /// </summary>
        public Vector3 dashDirection;

        /// <summary>
        /// This value is multiplied to a vector to give the dash speed.
        /// </summary>
        public float dashSpeed = 100;

        /// <summary>
        /// This function is used to give a timer to the dash
        /// </summary>
        public float dashDuration = 0.25f;

        /// <summary>
        /// How many seconds are left:
        /// </summary>
        private float dashTimer = 0;

        /// <summary>
        /// Hold reference to the charactercontroller 
        /// </summary>
        private CharacterController pawn;

        /// <summary>
        /// Set the original movestate to regular
        /// </summary>
        MoveState currentMoveState = MoveState.Regular;

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        /// <summary>
        /// update is called every game tick
        /// </summary>
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

        /// <summary>
        /// This function is called when the player tries to dash
        /// </summary>
        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * dashSpeed);
        }

        /// <summary>
        /// This funtion is called when the player uses the WASD keys
        /// </summary>
        /// <param name="mult"></param>
        private void MovePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagonal input vectors

            //pawn.Move(move * Time.deltaTime * playerSpeed * mult);
            pawn.SimpleMove(move * playerSpeed * mult);

        }
    }
}

