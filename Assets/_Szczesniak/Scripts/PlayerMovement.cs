using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class PlayerMovement : MonoBehaviour {

        public enum MoveState {
            Regular, // 0
            Dashing, // 1
            Sprinting, // 2
            Sneaking //3
        }

        public float playerSpeed = 10;

        private CharacterController pawn;

        MoveState currentMoveState = MoveState.Regular;
        // 1 = regular
        // 2 = dashing 
        // 3 = sneaking

        void Start() {
            pawn = GetComponent<CharacterController>();
        }


        void Update() {

            switch (currentMoveState) {
                case MoveState.Regular:

                    // do behaviour for this state:

                    MoveThePlayer(1);

                    // transitions to other states:
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;
                    if (Input.GetButtonDown("Fire1")) currentMoveState = MoveState.Sneaking;

                    break;
                case MoveState.Dashing:

                    // do behaviour for this state:

                    // transitions to other states:
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;

                    break;
                case MoveState.Sprinting:

                    // do behaviour for this state:

                    MoveThePlayer(2);

                    // transitions to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;

                    break;
                case MoveState.Sneaking:

                    // do behaviour for this state:

                    MoveThePlayer(.5f);

                    // transitions to other states:
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Regular;

                    break;
            }


        }

        private void MoveThePlayer(float mult = 1) {
            // Getting player input values of 0 to 1
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;

            //move.Normalize(); Doesn't work with gamepads like this also expensive

            if (move.sqrMagnitude > 1) move.Normalize(); // fix bug with diagonal input vectors

            pawn.Move(move * Time.deltaTime * playerSpeed * mult);
        }
    }
}