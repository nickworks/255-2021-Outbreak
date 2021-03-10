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
            Sneaking // 3
        }
        
        
        /// <summary>
        /// Playerspeed is the speed at which the player moves.
        /// </summary>
        public float playerSpeed = 10;
        /// <summary>
        /// pawn is the reference to our character controller.
        /// </summary>
        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regular;

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

            switch (currentMoveState)
            {
                case MoveState.Regular:

                    // Do behavior for this state:

                    MoveThePlayer(1);

                    // transition to other states:
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;


                    break;
                case MoveState.Dashing:

                    // Do behavior for this state:

                    

                    // transition to other states:

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
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;


                    break;
            }


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
