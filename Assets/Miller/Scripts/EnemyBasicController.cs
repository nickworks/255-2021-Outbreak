using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Miller
{
    public class EnemyBasicController : MonoBehaviour
    {
        public Projectile prefabProjectile;

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

            /// ///////////////// Child Classes

            public class Idle : State
            {

            }
            public class Patrolling : State
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

        private NavMeshAgent nav;

        public Transform attackTarget;

        // Start is called before the first frame update
        void Start()
        {
            nav = GetComponent<NavMeshAgent>();

        }

        // Update is called once per frame
        void Update()
        {
            if (attackTarget != null) nav.SetDestination(attackTarget.position);
            if (state == null) SwitchState(new States.Idle());
            if (state != null) SwitchState(state.Update());

        }

        void MoveTowardsTarget()
        {

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

