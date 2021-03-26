using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class PlayerMovement : MonoBehaviour
    {
        CharacterController controller;
        public float playerSpeed = 10f;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        // Update is called once per frame
        void Update()
        {
            // Behavior for this state:
            MoveThePlayer();
        }

        private void MoveThePlayer()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = (Vector3.right * h + Vector3.forward * v);

            if (move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagnoal input measures.

            controller.SimpleMove(move * playerSpeed);
        }
    }
}