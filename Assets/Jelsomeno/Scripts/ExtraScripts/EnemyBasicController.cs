using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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

        private NavMeshAgent nav;

        public Transform attackTarget;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

            //if(attackTarget != null) nav.SetDestination(attackTarget.position);
        }


        void Update()
        {

            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update());

            if (attackTarget != null) nav.SetDestination(attackTarget.position);

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
