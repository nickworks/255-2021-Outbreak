using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace _JSmith
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

        MoveState currentMoveState = MoveState.Regular;


        /// <summary>
        /// HOw long a dash should take, in seconds:
        /// </summary>
        public float dashDuration = 0.25f;

        /// <summary>
        /// How many m/s to move while dashing.
        /// </summary>
        public float dashSpeed = 50;

        private Vector3 dashDirection;

        /// <summary>
        /// This stores how many seconds are left:
        /// </summary>
        private float dashTimer = 0;


        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        void Update()
        {

            switch (currentMoveState)
            {
                case MoveState.Regular:

                    //do behavior for this state:

                    MoveThePlayer(1);

                    //transitions to other states:
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButtonDown("Fire2"))
                    { //transition to dashing
                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxisRaw("Horizontal"); // -1 / 0 / 1
                        float v = Input.GetAxisRaw("Vertical"); // -1 or 0 or 1
                        dashDirection = new Vector3(h, 0, v);
                        dashDirection.Normalize();
                        dashTimer = .1f;

                        //clamp the length of dashDir to 1.
                        if (dashDirection.sqrMagnitude > 1) dashDirection.Normalize();
                    }

                    break;
                case MoveState.Dashing:
                    //do behavior for this state:
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;
                    //transitions to other states:
                    if (dashTimer <= 0) currentMoveState = MoveState.Regular;
                    break;
                case MoveState.Sprinting:

                    //do behavior for this state:

                    MoveThePlayer(2);

                    //transitions to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    //if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;

                    break;
                case MoveState.Sneaking:

                    //do behavior for this state:

                    MoveThePlayer(.5f);

                    //transitions to other states:
                    //if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;
                    break;
            }




        }

        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * dashSpeed);
        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix bug with diagonal input vectors

            //pawn.Move(move * Time.deltaTime * playerSpeed * mult);
            pawn.SimpleMove(move * playerSpeed * mult);

        }
    }
}
