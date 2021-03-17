using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Foster
{
    public class PlayerMovement : MonoBehaviour
    {
        public enum MoveState 
        {
                Regular,
                Dashing,
                Sprinting,
                Sneaking,
        }

        public float playerSpeed = 10;

        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regular;

        /// <summary>
        /// How long a dash should take in seconds
        /// </summary>
        public float dashDuration = .25f;
        private Vector3 dashDirection;
        /// <summary>
        /// This stores how many seconds are left
        /// </summary>
        private float dashTimer = 0;

        void Start()
        {

            pawn = GetComponent<CharacterController>();

        }

        // Update is called once per frame
        void Update()
        {
            switch (currentMoveState)
            {
                case MoveState.Regular:

                    //do behavioyr for this state;
                    MoveThePlayer(1);


                    //transition to other states;
                    if (Input.GetButtonDown("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButtonDown("Fire1")) currentMoveState = MoveState.Sneaking;

                    if (Input.GetButtonDown("Fire2")) //transition into dashing
                    {

                        currentMoveState = MoveState.Dashing;
                        float h = Input.GetAxis("Horizontal");
                        float v = Input.GetAxis("Vertical");
                        dashDirection = new Vector3(h, 0, v);

                    }

                    break;

                case MoveState.Sneaking:

                    //do behavioyr for this state;
                    MoveThePlayer(.5f);


                    //transition to other states;
                    if (!Input.GetButtonDown("Fire1")) currentMoveState = MoveState.Regular;

                    break;
                case MoveState.Sprinting:

                    //do behavioyr for this state;
                    MoveThePlayer(2);


                    //transition to other states;
                    if (!Input.GetButtonDown("Fire3")) currentMoveState = MoveState.Regular;
                    if (Input.GetButtonDown("Fire1")) currentMoveState = MoveState.Sneaking;


                    break;
                case MoveState.Dashing:

                    //do behavioyr for this state;
                    DashThePlayer();

                    dashTimer -= Time.deltaTime;

                    //transition to other states;


                    break;
            }

        }
        private void DashThePlayer()
        {
            pawn.Move(dashDirection * Time.deltaTime * 100);
        }
        private void MoveThePlayer(float mult)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;
            if(move.sqrMagnitude >1) move.Normalize(); // fix bug with diaginal input vectors

            pawn.Move(move * Time.deltaTime * playerSpeed * mult);


        }
    }
}