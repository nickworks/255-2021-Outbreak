using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class EnemyController : MonoBehaviour
    {
        class States
        {
            public class State
            {
                protected EnemyController enemy;
                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart()
                {

                }
                virtual public void OnEnd()
                {

                }
            }
            public class Idle : State { }
            public class Pursuing : State { }
            public class Patrolling : State { }
            public class Stunned : State { }
            public class Death : State { }
            public class Attack1 : State { }
            public class Attack2 : State { }
            public class Attack3 : State { }

        }

        private States.State state;

        // EnemyBasicController.States.State

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (state == null) SwitchState(new States.Idle());

            if (state!=null)state.Update();
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return; // don't switch to anything...
            if(state!=null)state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);
        }
    }
}