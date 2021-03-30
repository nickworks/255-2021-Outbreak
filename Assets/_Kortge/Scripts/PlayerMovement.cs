using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves the player character based on input.
/// </summary>
namespace Kortge
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// The component used to move the character.
        /// </summary>
        CharacterController controller;

        /// <summary>
        /// Gets the controller component.
        /// </summary>
        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }
        /// <summary>
        /// Moves the player each frame.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            // Behavior for this state:
            MoveThePlayer();
        }
        /// <summary>
        /// Moves the player horizontally, vertically, or diagonally based on player input.
        /// </summary>
        private void MoveThePlayer()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = (Vector3.right * h + Vector3.forward * v);

            if (move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagnoal input measures.

            controller.SimpleMove(move * 10);
        }
    }
}