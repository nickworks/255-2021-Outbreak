using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins {
    public class PlayerMovement : MonoBehaviour
    {
        public float playerSpeed = 10;

        private CharacterController pawn;

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix diagonal movement bug

            pawn.Move(move * Time.deltaTime * playerSpeed);

        }
    }
}