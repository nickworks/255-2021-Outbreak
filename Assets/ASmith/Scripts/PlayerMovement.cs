using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class PlayerMovement : MonoBehaviour
    {
        public float playerSpeed = 10;

        private CharacterController pawn;
        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v; // Calculates what direction to walk in

            if (move.sqrMagnitude > 1) move.Normalize(); // Fixes bug that makes diagonal movement faster

            pawn.Move(move * Time.deltaTime * playerSpeed); // Moves the player based on created variables
        }
    }
}

