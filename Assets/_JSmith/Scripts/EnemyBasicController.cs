using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace _JSmith
{
    public class EnemyBasicController : MonoBehaviour
    {

        static class States
        {
            public class state
            {
                protected EnemyBasicController enemy;

                virtual public state Update()
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

            public class Idle : state
            {

            }
            public class Pursuing : state
            {

            }
            public class Stunned : state
            {

            }
            public class Death : state
            {

            }
            public class Attack1 : state
            {

            }
            public class Attack2 : state
            {

            }
            public class Attack3 : state
            {

            }
        }

        private States.state state;

        private NavMeshAgent nav;

        public Transform attackTarget;

        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

        }


        void Update()
        {
            if(attackTarget != null) nav.SetDestination(attackTarget.position);

            if (state == null) SwitchState(new States.Idle());

            if(state != null) SwitchState(state.Update());
        }
    
        void SwitchState(States.state newState)
        {
            if (newState == null) return; // don't switch to nothing...

            if(state != null) state.OnEnd(); //tell previous state it is done.
            state = newState; //swap states
            state.OnStart(this);
        }
    }
}
