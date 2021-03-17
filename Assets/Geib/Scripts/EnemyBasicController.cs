using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geib
{
    public class EnemyBasicController : MonoBehaviour
    {
       static  class States
        {
            
            /////////////////// Child States
            public class State
            {

                protected EnemyBasicController enemy;

                virtual public State Update()
                {
                    
                    return null;
                }
                virtual public void OnStart(EnemyBasicController enemy)
                {
                    this.enemy = enemy;
                }
                virtual public void OnEnd()
                {

                }
            }

            public class Idle : State
            {

            }
            public class Pursuing : State
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

        /// <summary>
        /// Holds the current state
        /// </summary>
        private States.State state;



        void Start()
        {

        }


        void Update()
        {
            if (state == null) SwitchState(new States.Idle()); 
            if (state != null) SwitchState(state.Update());
        }
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // Don't switch to nothing...
            if(state != null) state.OnEnd();// Tell the previous state it is done
            state = newState; // Swap state
            state.OnStart(this);
        }
    }
}
