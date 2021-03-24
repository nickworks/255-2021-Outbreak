using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class BossStates : MonoBehaviour
    {
        static class States
        {
            public class State
            {
                protected BossStates boss;

                virtual public State Update()
                {
                    return null;
                }

                virtual public void OnStart(BossStates boss)
                {
                    this.boss = boss;
                }

                virtual public void OnEnd()
                {

                }
            }

            ////////////////// Children of the State class

            public class Idle : State
            {

            }
            public class Persuing : State
            {

            }
            public class ClimbWall : State
            {

            }
            public class Stunned : State
            {

            }
            public class Death : State
            {

            }
            public class Attack1 : State
            {

            }
            public class Attack2 : State
            {

            }
            public class Attack3 : State
            {

            }
        }

        private States.State state;

        void Start()
        {

        }


        void Update()
        {
            // If we are not in a state, use the Idle state
            if (state == null) SwitchState(new States.Idle());

            // If we are in a state, look for the switch condition in the current state's update
            if (state != null) SwitchState(state.Update());
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return; // Can't switch to nothing

            // Call the previous state's on end 
            if (state != null) state.OnEnd();

            // Switch to the new state
            state = newState;

            // Call the new state's on start function
            state.OnStart(this);
        }
    }
}

