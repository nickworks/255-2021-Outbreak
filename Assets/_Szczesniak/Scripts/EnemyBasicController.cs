using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Szczesniak {
    /// <summary>
    /// This class controls all of the minion's behaviours
    /// </summary>
    public class EnemyBasicController : MonoBehaviour {

        /// <summary>
        /// The state pattern class
        /// </summary>
        static class States {
            public class State {

                /// <summary>
                /// To get access outside of this child class, boss is needed to access outside variables.
                /// </summary>
                protected EnemyBasicController enemy;

                /// <summary>
                /// Sets update up.
                /// </summary>
                /// <returns></returns>
                virtual public State Update() {
                    
                    return null;
                }

                /// <summary>
                /// Referencing EnemyBasicController
                /// </summary>
                /// <param name="enemy"></param>
                virtual public void OnStart(EnemyBasicController enemy) {
                    this.enemy = enemy;
                }

                /// <summary>
                /// Tell when it is done
                /// </summary>
                virtual public void OnEnd() { 

                }
            }
            
            //////////////////////////// Child Classes: 
            
            /// <summary>
            /// When the minion is not doing anything
            /// </summary>
            public class Idle : State {
                public override State Update() {
                    // Transitions:
                    // if the minion health is at or below 0 or the boss's health is at 0 or below
                    if (enemy.bossHealth.health <= 0 || enemy.enemyBasicHealth.health <= 0) {
                        enemy.DeathPhase(); // starts death phase
                        return new States.Death(); // goes to Death state
                    }

                    if (enemy.CanSeeThing(enemy.attackTarget, enemy.viewingDistance)) // if the enemy can see the player
                        return new States.Pursuing(); // goes to pursuing state

                    return null;
                }
            }

            /// <summary>
            /// Enemy pursuing state chases and attacks the player
            /// </summary>
            public class Pursuing : State {

                public override State Update() {
                    // Behaviour:
                    enemy.MoveTowardTarget(); // moves toward target/player


                    // Transition:
                    // if the minion health is at or below 0 or the boss's health is at 0 or below
                    if (enemy.bossHealth.health <= 0 || enemy.enemyBasicHealth.health <= 0) { 
                        enemy.DeathPhase(); // starts death phase
                        return new States.Death(); // goes to Death state
                    }

                    enemy.dashTimer -= Time.deltaTime; // counts down to be able to dash
                    // checks to see if the player is in range for a dash attack and if the timer is at 0 
                    if (enemy.CanSeeThing(enemy.attackTarget, enemy.targetDistanceToDash) && enemy.dashTimer <= 0) {
                        enemy.dashTimer = 5; // resets dash timer
                        SoundEffectBoard.DashSound(); // plays dash sound
                        return new States.DashAttack(); // goes to dash attack state
                    }

                    return null;
                }


            }

            /// <summary>
            /// Death state does nothing to make sure the minion does not move and or do any action
            /// </summary>
            public class Death : State {

            }

            /// <summary>
            /// Runs this state when the dash attack is switched to this
            /// </summary>
            public class DashAttack : State {

                public override State Update() {
                    // Behaviour:
                    enemy.DashAttack(); // does the dash attack

                    // Transition:
                    enemy.dashDuration -= Time.deltaTime; // how long to let it dash
                    if (enemy.dashDuration <= 0) { // if dash duration is at 0 or below
                        enemy.dashDuration = .5f; // re-sets dash duration to half a second
                        return new States.Idle(); // goes back to Idle
                    }

                    return null;
                }
            }

        }

        // EnemyBasicController.States.State

        /// <summary>
        /// access the state pattern, maintain it, and make it function.
        /// </summary>
        private States.State state;

        /// <summary>
        /// controls the minion's navigation across the map
        /// </summary>
        private NavMeshAgent nav;

        /// <summary>
        /// The location of the player to attack at.
        /// </summary>
        public GameObject attackTarget;

        /// <summary>
        /// Distance the minion can see
        /// </summary>
        public float viewingDistance = 10;

        /// <summary>
        /// Angle the minion can see
        /// </summary>
        public float viewingAngle = 35;

        /// <summary>
        /// How long until the minion can dash again.
        /// </summary>
        public float dashTimer = 5;

        /// <summary>
        /// How long the dash is.
        /// </summary>
        public float dashDuration = .5f;

        /// <summary>
        /// How far until the minion inisiate the dash attack
        /// </summary>
        public float targetDistanceToDash = 6;

        /// <summary>
        /// Getting boss to check its health
        /// </summary>
        GameObject bossObject;

        /// <summary>
        /// Getting boss health
        /// </summary>
        private HealthScript bossHealth;

        /// <summary>
        /// Getting minion's health for itself
        /// </summary>
        private HealthScript enemyBasicHealth;

        /// <summary>
        /// random number to count down to destroy itself when the player kills the boss
        /// </summary>
        int timeLeftToDestroy = 0;

        /// <summary>
        /// Explosion particles when the minion dies
        /// </summary>
        public ParticleSystem enemyExplosion;

        private int chanceToSpawnHealth;
        public Transform healthItem;


        void Start() {
            nav = GetComponent<NavMeshAgent>(); // Gets navMeshAgent to move around
            attackTarget = GameObject.FindGameObjectWithTag("Player"); // Gets player gameObject
            bossObject = GameObject.FindGameObjectWithTag("Boss"); // Gets boss gameObject
            bossHealth = bossObject.GetComponent<HealthScript>(); // gets boss's health
            enemyBasicHealth = GetComponent<HealthScript>(); // gets its health
            timeLeftToDestroy = Random.Range(5, 15); // generates random number for timer

            chanceToSpawnHealth = Random.Range(1, 4);
        }

        void Update() {

            // if nothing is assigned to the state, then make the state go to the Idle() state
            if (state == null) SwitchState(new States.Idle());
            //if (state == null) state = 

            if (state != null) SwitchState(state.Update()); // makes the state run it's update method


        }

        /// <summary>
        /// Move nav mesh agent (the minion) towards the player.
        /// </summary>
        void MoveTowardTarget() {
            if (attackTarget) nav.SetDestination(attackTarget.transform.position); // sets point for minion to go to
        }

        /// <summary>
        /// Makes the state swtich to a different state
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState) {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);
        }

        /// <summary>
        /// Dash attack that moves minion forward faster at player
        /// </summary>
        void DashAttack() {
            Vector3 dash = transform.forward * 20; // gets vector to move forward
            transform.position += dash * Time.deltaTime; // moves minion forward at player

        }

        /// <summary>
        /// Calculation for turret to see targets
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        private bool CanSeeThing(GameObject thing, float viewingDistance) {

            if (!thing) return false; // uh... error

            Vector3 vToThing = thing.transform.position - transform.position; // distance from minion to target

            // check distance
            if (vToThing.sqrMagnitude > viewingDistance * viewingDistance) return false; // Too far away to see...

            // check direction
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // TODO: Check occulusion

            return true;
        }

        /// <summary>
        /// When the minion hits a target
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter(Collider collision) {
            HealthScript healthOfPlayer = collision.GetComponent<HealthScript>();
            if (collision.gameObject.tag == "Player" && healthOfPlayer) {
                healthOfPlayer.DamageTaken(15);
            }
        }
        
        /// <summary>
        /// When the minion losses all health
        /// </summary>
        void DeathPhase() {
            if (chanceToSpawnHealth == 1) Instantiate(healthItem, transform.position, Quaternion.identity);
            Instantiate(enemyExplosion, transform.position, enemyExplosion.transform.rotation); // instantiates the explosion
            nav.enabled = false; // stops minion
            Destroy(gameObject, timeLeftToDestroy); // destroy minion at a certain time
        }
    }
}