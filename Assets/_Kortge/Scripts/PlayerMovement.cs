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

        MoveState currentMoveState = MoveState.Regular;

        // Start is called before the first frame update
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
                    // Behavior for this state:
                    MoveThePlayer(1);
                    // Transitions for other states.
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    break;

                case MoveState.Dashing:
                    // Behavior for this state:
                    // Transitions for other states.
                    break;

                case MoveState.Sprinting:
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

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagnoal input measures.

            pawn.Move(move * Time.deltaTime * playerSpeed);
        }
    }
}