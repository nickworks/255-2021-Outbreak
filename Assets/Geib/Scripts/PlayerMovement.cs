using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Geib
{
    public class PlayerMovement : MonoBehaviour
    {
        /// <summary>
        /// Playerspeed is the speed at which the player moves.
        /// </summary>
        public float playerSpeed = 10;
        /// <summary>
        /// pawn is the reference to our character controller.
        /// </summary>
        private CharacterController pawn;

        /// <summary>
        /// Runs on the first tick of the game.
        /// </summary>
        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Runs every frame of the game.
        /// </summary>
        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");


            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix bug with diagonal input vectors.
            

            pawn.Move(move * Time.deltaTime * playerSpeed);

        }
    }
}
