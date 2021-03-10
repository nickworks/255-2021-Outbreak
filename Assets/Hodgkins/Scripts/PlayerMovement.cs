using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins {
    public class PlayerMovement : MonoBehaviour
    {
        public enum MoveState
        {
            Regualar, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking // 3
        }
        
        
        
        public float playerSpeed = 10;

        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regualar;

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (currentMoveState)
            {
                case MoveState.Regualar:
                    // do behaviour for this state
                    MoveThePlayer(1);
                    // transition to other state
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    break;
                case MoveState.Dashing:
                    break;
                case MoveState.Sprinting:
                    // do behaviour for this state
                    MoveThePlayer(2);
                    // transition to other state
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regualar;
                    break;
                case MoveState.Sneaking:
                    // do behaviour for this state
                    MoveThePlayer(0.5f);
                    // transition to other state
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regualar;
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;

                    break;
            }



        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix diagonal movement bug

            pawn.Move(move * Time.deltaTime * playerSpeed * mult);
        }
    }
}