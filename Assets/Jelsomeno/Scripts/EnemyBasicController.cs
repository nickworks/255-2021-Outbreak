using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
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


            public class Idle : State
            {

            }

            public class Pursue : State
            {

            }

            public class Death : State
            {

            }

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
            if (newState == null) return;


            if (state != null) state.OnEnd();
            state = newState;
            state.OnStart(this);
        }

    }

}
