using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class PlayerMovement : MonoBehaviour
    {
        public float playerSpeed = 10;

        private CharacterController pawn;

        // Start is called before the first frame update
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

            if(move.sqrMagnitude > 1) move.Normalize(); // Fix bug with diagnoal input measures.

            pawn.Move(move*Time.deltaTime*playerSpeed);
        }
    }
}