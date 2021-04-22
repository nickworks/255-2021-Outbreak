using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// State Machine for the Player's Movement
        /// </summary>
        public enum MoveState 
        {
            Regular, // 0
            Dashing, // 1
        }

        /// <summary>
        /// Variable containing the player's movement speed
        /// </summary>
        public float playerSpeed = 10;

        /// <summary>
        /// Stores how many seconds left in dash
        /// </summary>
        private float dashTimer = 0;

        /// <summary>
        /// How strong the dash is
        /// The higher the number, the stronger the dash
        /// </summary>
        public float dashSpeed = 10;

        /// <summary>
        /// How many dashes are available
        /// </summary>
        public static int dashCounter = 2;

        /// <summary>
        /// Variable that tracks when the player can dash again
        /// </summary>
        private float dashCooldown = 0;

        /// <summary>
        /// Variable containing the maximum amount of times
        /// the player can dash before it must recharge
        /// </summary>
        private float maxDashes = 2;

        /// <summary>
        /// Variable containing the player pawn
        /// </summary>
        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regular;

        /// <summary>
        /// Variable containing the intended direction of the dash
        /// </summary>
        private Vector3 dashDirection;        

        void Start()
        {
            pawn = GetComponent<CharacterController>(); // Gets  reference to the Character Controller to be used in the dash ability
        }

        void Update()
        {
            if (dashCounter < 2) // If the dash count is LESS THAN 2...
            {
                dashCooldown -= Time.deltaTime; // Start dash recharge

                if (dashCooldown <= 0) // If the dash cooldown is LESS THAN or EQUAL TO 0
                {
                    dashCounter++; // Increash dash count by 1
                    dashCooldown = 2; // Restart the dash cooldown
                }
            }

            switch (currentMoveState) // State Switcher
            {
                case MoveState.Regular: // Regular State: Player Moving around

                    // Do behavior for this state:
                    MoveThePlayer(1);

                    // Transition to other states:
                    if (dashCounter > 0 && Input.GetButtonDown("Fire2")) // On right mouse down, go to dash state
                    {
                        currentMoveState = MoveState.Dashing;
                        dashCounter--;
                        float h = Input.GetAxisRaw("Horizontal"); // Raw makes these number default to wholes rather than decimals: -1/0/1
                        float v = Input.GetAxisRaw("Vertical"); // Raw makes these number default to wholes rather than decimals: -1/0/1
                        dashDirection = new Vector3(h, 0, v); // Ties the dash vector to "h" and "v" for the x and z axis respectively
                        dashDirection.Normalize();
                        dashTimer = .25f; // Restarts the dash timer
                        dashCooldown = 2; // Restarts the dash cooldown

                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize(); // Clamps the length of dash to 1 so diagonal movement is same length
                    }

                    break;

                case MoveState.Dashing: // Dashing State: Player right clicks to dash

                    // Do behavior for this state:
                    DashThePlayer();

                    dashTimer -= Time.deltaTime; // Counts down dashTimer

                    // Transition to other states:
                    if (dashTimer <= 0) currentMoveState = MoveState.Regular; // If dashTimer is LESS THAN or EQUAL TO 0, switch to Regular State

                    break;
            }
        }

        /// <summary>
        /// This function moves the player while they are dashing
        /// </summary>
        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * 2.5f * dashSpeed); // Calculates where to dash the player and how fast
            SoundBoard.PlayPlayerDash(); // Play the Dash sound
        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal"); // sets a variable for the Horizontal axis
            float v = Input.GetAxis("Vertical"); // sets a variable for the Vertical axis

            Vector3 move = Vector3.right * h + Vector3.forward * v; // Calculates what direction to walk in

            if (move.sqrMagnitude > 1) move.Normalize(); // Fixes bug that makes diagonal movement faster

            pawn.SimpleMove(move * playerSpeed * mult); // using simple move doesn't need Delta Time and also applies gravity automatically
        }
    }
}

