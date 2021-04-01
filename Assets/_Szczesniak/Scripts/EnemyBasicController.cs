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

            public class MeleeAttack : State {

            }

            public class DashAttack : State {

            }

            public class SelfDestruct : State {

            }

        }

        // EnemyBasicController.States.State

        private States.State state;

        private NavMeshAgent nav;

        public GameObject attackTarget;

        public float viewingDistance = 10;
        public float viewingAngle = 35;

        void Start() {
            nav = GetComponent<NavMeshAgent>();
            attackTarget = GameObject.FindGameObjectWithTag("Player");
            
        }

        void Update() {

            if (state == null) SwitchState(new States.Idle());
            //if (state == null) state = 

            if (state != null) SwitchState(state.Update());

            if (CanSeeThing(attackTarget))
                MoveTowardTarget();
        }

        void MoveTowardTarget() {
            if (attackTarget) nav.SetDestination(attackTarget.transform.position);
        }

        void SwitchState(States.State newState) {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);
        }

        /// <summary>
        /// Calculation for turret to see targets
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        private bool CanSeeThing(GameObject thing) {

            if (!thing) return false; // uh... error

            Vector3 vToThing = thing.transform.position - transform.position;

            // check distance
            if (vToThing.sqrMagnitude > viewingDistance * viewingDistance) return false; // Too far away to see...

            // check direction
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // TODO: Check occulusion

            return true;
        }
    }
}