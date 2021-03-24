using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class EnemyBasicController : MonoBehaviour
    {
        static class States
        {
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

            //////////////////////// Children of State
            public class Idle : State { }
            public class Persuing : State { }
            public class Patrolling : State { }
            public class Stunned : State { }
            public class Death : State { }
            public class Attack1 : State { }
            public class Attack2 : State { }
            public class Attack3 : State { }
        }

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

            // Call the current state's onEnd function.
            if (state != null) state.OnEnd();

            // Switch the state.
            state = newState;
            
            // Call the new state's onStart function.
            state.OnStart(this);
        }
    }
}

