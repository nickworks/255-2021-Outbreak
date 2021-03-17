using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Geib
{
    public class PlayerMovement : MonoBehaviour
    {
        
        /// <summary>
        /// This enum carries all of the possible states.
        /// </summary>
        public enum MoveState
        {
            Regular, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking, // 3
            Shielding
        }
        
        
        /// <summary>
        /// Playerspeed is the speed at which the player moves.
        /// </summary>
        public float playerSpeed = 10;
        /// <summary>
        /// pawn is the reference to our character controller.
        /// </summary>
        private CharacterController pawn;
        /// <summary>
        /// This variable stores the current move state.
        /// </summary>
        MoveState currentMoveState = MoveState.Regular;
        /// <summary>
        /// This variable holds the direction the player was facing when they initiated the dash action.
        /// </summary>
        private Vector3 dashDirection;
        /// <summary>
        /// How long the dash should take, in seconds:
        /// </summary>
        public float dashDuration = 0.25f;
        /// <summary>
        /// This stores how many seconds are left:
        /// </summary>
        public float dashTimer = 0;
        /// <summary>
        /// Runs on the first tick of the game.
        /// </summary>
        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Runs every frame of the game.
        /// </summary>
        void Update()
        {

            Debug.Log(currentMoveState);
            switch (currentMoveState)
            {
                case MoveState.Regular:

                    // Do behavior for this state:

                    MoveThePlayer(1);

                    // transition to other states:
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButton("Fire2")) // Transition into Dashing
                    {
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxis("Horizontal");
                        float v = Input.GetAxis("Vertical");
                        dashDirection = new Vector3(h, 0, v); // Vertical axis is on the Z-axis because the game is two dimensional and top-down
                        // Clamps the length of dashDir to 1:
                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize();
                    }


                    break;
                case MoveState.Dashing:

                    // Do behavior for this state:
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;

                    // transition to other states:
                    if (dashTimer <= 0 ) currentMoveState = MoveState.Regular;

                    break;
                case MoveState.Sprinting:


                    // Do behavior for this state:

                    MoveThePlayer(2);

                    // transition to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;



                    break;
                case MoveState.Sneaking:

                    // Do behavior for this state:

                    MoveThePlayer(0.5f);

                    // transition to other states:
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;


                    break;
            }

        }

        /// <summary>
        /// This function holds the logic for the player's dash state
        /// </summary>
        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * 100);
        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix bug with diagonal input vectors.


            pawn.Move(move * Time.deltaTime * playerSpeed * mult);
        }
    }
}
