using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class PlayerMovement : MonoBehaviour {

        public float playerSpeed = 10;

        private CharacterController pawn;

        void Start() {
            pawn = GetComponent<CharacterController>();
        }


        void Update() {
            // Getting player input values of 0 to 1
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;

            //move.Normalize(); Doesn't work with gamepads like this also expensive

            if (move.sqrMagnitude > 1) move.Normalize(); // fix bug with diagonal input vectors

            pawn.Move(move * Time.deltaTime * playerSpeed);
        }
    }
}