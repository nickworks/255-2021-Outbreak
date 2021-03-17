using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
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

        private CharacterController pawn;

        /// <summary>
        /// How long a dash should take, in seconds:
        /// </summary>
        public float dashDuration = 0.25f;

        public float dashTimer = 0.25f;

        private Vector3 dashDirection;
        /// <summary>
        /// This stores how many seconds are left:
        /// </summary>

        MoveState currentMoveState = MoveState.Regular;

        // Start is called before the first frame update
        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            print(currentMoveState);
            switch (currentMoveState)
            {
                case MoveState.Regular:
                    // Behavior for this state:
                    MoveThePlayer(1);
                    // Transitions for other states.
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButton("Fire2"))
                    {
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxis("Horizontal");
                        float v = Input.GetAxis("Vertical");
                        dashDirection = new Vector3(h, 0, v);
                        // clamp the length of dashDir to 1:
                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize();
                    }

                    break;

                case MoveState.Dashing:
                    // Behavior for this state:
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;

                    // Transitions for other states.
                    if (dashTimer <= 0) currentMoveState = MoveState.Regular;

                    break;

                case MoveState.Sprinting:
                    DashThePlayer();
                    // Behavior for this state:
                    MoveThePlayer(2);
                    // Transitions for other states.
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    break;
                
                case MoveState.Sneaking:
                    // Behavior for this state:
                    MoveThePlayer(0.5f);
                    // Transitions for other states.
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    break;
            }
        }

        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * 100);
        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = (Vector3.right * h + Vector3.forward * v) * mult;

            if (move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagnoal input measures.

            pawn.Move(move * Time.deltaTime * playerSpeed);
        }
    }
}