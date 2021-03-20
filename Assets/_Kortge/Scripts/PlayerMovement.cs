using System;
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

        /// <summary>
        /// How long a dash should take, in seconds:
        /// </summary>
        public float dashDuration = 0.25f;

        public float dashTimer = 0.25f;

        private Vector3 dashDirection;
        /// <summary>
        /// This stores how many seconds are left:
        /// </summary>

        MoveState currentMoveState = MoveState.Regular;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            // Behavior for this state:
            MoveThePlayer(1);
        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = (Vector3.right * h + Vector3.up * v) * mult;

            if (move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagnoal input measures.

            transform.position += move * Time.deltaTime * playerSpeed;
        }
    }
}