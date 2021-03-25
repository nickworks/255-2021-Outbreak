using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Szczesniak {

    public class EnemyBasicController : MonoBehaviour {

        static class States {
            public class State {

                protected EnemyBasicController enemy;

                virtual public State Update() {
                    
                    return null;
                }

                virtual public void OnStart(EnemyBasicController enemy) {
                    this.enemy = enemy;
                }

                virtual public void OnEnd() { 

                }
            }
            
            //////////////////////////// Child Classes: 
            
            public class Idle : State {

            }

            public class Pursuing : State {

            }

            public class Patrolling : State {

            }

            public class Death : State {

            }

            public class Stunned : State {

            }

            public class Attack1 : State {

            }

            public class Attack2 : State {

            }

            public class Attack3 : State {

            }

        }

        // EnemyBasicController.States.State

        private States.State state;

        private NavMeshAgent nav;

        public Transform attackTarget;

        void Start() {
            nav = GetComponent<NavMeshAgent>();

            
        }

        void Update() {

            if (state == null) SwitchState(new States.Idle());
            //if (state == null) state = 

            if (state != null) SwitchState(state.Update());

            MoveTowardTarget();
        }

        void MoveTowardTarget() {
            if (attackTarget) nav.SetDestination(attackTarget.position);
        }

        void SwitchState(States.State newState) {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);
        }
    }
}