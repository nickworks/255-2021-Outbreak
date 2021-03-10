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
            Sneaking // 3
        }

        MoveState currentMoveState = MoveState.Regular;

        public float playerSpeed = 10;

        private CharacterController pawn;

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        void Update()
        {
            switch (currentMoveState)
            {
                case MoveState.Regular:

                    // Do behavior for this state:
                    MoveThePlayer(1);

                    // Transition to other states:
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting; // When holding shift start sprinting state
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking; // When holding ctrl go to sneak state

                    break;
                case MoveState.Dashing:

                    // Do behavior for this state:
                    
                    // Transition to other states:

                    break;
                case MoveState.Sprinting:

                    // Do behavior for this state:
                    MoveThePlayer(2);

                    // Transition to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular; // When not holding shift return to regular state
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Regular; // When holding ctrl go to sneak state

                    break;
                case MoveState.Sneaking:

                    // Do behavior for this state:
                    MoveThePlayer(.5f);

                    // Transition to other states:
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular; // When not holding ctrl return to regular state

                    break;
            }
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

