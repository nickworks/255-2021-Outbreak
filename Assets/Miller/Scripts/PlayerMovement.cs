using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Miller
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

        private CharacterController pawn;

        int currentMoveState = 1;
        //1 = regular
        //2 = dashing
        //3 = sneaking

        void Start()
        {
            pawn = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

            switch (currentMoveState)
            {
                case MoveState.Regular:

                    // do behavior for this state:

                    MoveThePlayer(1);

                    // transitions to other states:
                    if (Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;
                    if (Input.GetButton("Fire3")) currentMoveState = MoveState.Sprinting;

                    break;
                case MoveState.Dashing:

                    // do behavior for this state:

                    // transitions to other states:

                    break;
                case MoveState.Sprinting:

                    // do behavior for this state:

                    MoveThePlayer(2);


                    // transitions to other states:
                    if (!Input.GetButton("Fire3")) currentMoveState = MoveState.Regular;
                    if (!Input.GetButton("Fire1")) currentMoveState = MoveState.Sneaking;

                    break;
                case MoveState.Sneaking:

                    // do behavior for this state:

                    // transitions to other states:

                    break;
            }

        }

        private void MoveThePlayer(float mult = 1)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 move = Vector3.right * h + Vector3.forward * v;

            if (move.sqrMagnitude > 1) move.Normalize(); // fix diagonal input vectors

            pawn.Move(move * Time.deltaTime * playerSpeed);

        }
    }
}
