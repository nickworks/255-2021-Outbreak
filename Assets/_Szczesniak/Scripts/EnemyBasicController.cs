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
                public override State Update() {
                    // Behaviour:

                    // Transitions:
                    if (enemy.bossHealth.health <= 0 || enemy.enemyBasicHealth.health <= 0) {
                        enemy.DeathPhase();
                        return new States.Death();
                    }

                    if (enemy.CanSeeThing(enemy.attackTarget, enemy.viewingDistance))
                        return new States.Pursuing();

                    return null;
                }
            }

            public class Pursuing : State {

                public override State Update() {
                    // Behaviour:
                    enemy.MoveTowardTarget();


                    // Transition:
                    if (enemy.bossHealth.health <= 0 || enemy.enemyBasicHealth.health <= 0) {
                        enemy.DeathPhase();
                        return new States.Death();
                    }

                    enemy.dashTimer -= Time.deltaTime;
                    if (enemy.CanSeeThing(enemy.attackTarget, enemy.targetDistanceToDash) && enemy.dashTimer <= 0) {
                        enemy.dashTimer = 5;
                        return new States.DashAttack();
                    }

                    return null;
                }


            }

            public class Patrolling : State {

            }

            public class Death : State {

                public override State Update() {
                    

                    return null;
                }

            }

            public class Stunned : State {

            }

            public class MeleeAttack : State {

            }

            public class DashAttack : State {

                public override State Update() {
                    // Behaviour:
                    enemy.DashAttack();

                    // Transition:
                    enemy.dashDuration -= Time.deltaTime;
                    if (enemy.dashDuration <= 0) {
                        enemy.dashDuration = .5f;
                        return new States.Idle();
                    }

                    return null;
                }
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

        public float dashTimer = 5;
        public float dashDuration = .5f;
        public float targetDistanceToDash = 6;

        GameObject bossObject;
        private HealthScript bossHealth;
        private HealthScript enemyBasicHealth;
        int timeLeftToDestroy = 0;

        public ParticleSystem enemyExplosion;


        void Start() {
            nav = GetComponent<NavMeshAgent>();
            attackTarget = GameObject.FindGameObjectWithTag("Player");
            bossObject = GameObject.FindGameObjectWithTag("Boss");
            bossHealth = bossObject.GetComponent<HealthScript>();
            enemyBasicHealth = GetComponent<HealthScript>();
            timeLeftToDestroy = Random.Range(5, 15);
        }

        void Update() {

            if (state == null) SwitchState(new States.Idle());
            //if (state == null) state = 

            if (state != null) SwitchState(state.Update());


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

        void DashAttack() {
            Vector3 dash = transform.forward * 20;

            transform.position += dash * Time.deltaTime;

        }

        /// <summary>
        /// Calculation for turret to see targets
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        private bool CanSeeThing(GameObject thing, float viewingDistance) {

            if (!thing) return false; // uh... error

            Vector3 vToThing = thing.transform.position - transform.position;

            // check distance
            if (vToThing.sqrMagnitude > viewingDistance * viewingDistance) return false; // Too far away to see...

            // check direction
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // TODO: Check occulusion

            return true;
        }

        private void OnTriggerEnter(Collider collision) {
            HealthScript healthOfPlayer = collision.GetComponent<HealthScript>();
            if (collision.gameObject.tag == "Player" && healthOfPlayer) {
                healthOfPlayer.DamageTaken(5);
            }
        }

        void DeathPhase() {
            Instantiate(enemyExplosion, transform.position, enemyExplosion.transform.rotation);
            nav.isStopped = true;
            nav.enabled = false;
            Destroy(gameObject, timeLeftToDestroy);
        }
    }
}