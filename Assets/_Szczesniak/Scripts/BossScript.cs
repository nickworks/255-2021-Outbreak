using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Szczesniak {
    /// <summary>
    /// This Class makes uses state pattern for the boss's movement
    /// </summary>
    public class BossScript : MonoBehaviour {

        /// <summary>
        /// The state pattern class
        /// </summary>
        static class States {
            public class State {

                /// <summary>
                /// To get access outside of this child class, boss is needed to access outside variables.
                /// </summary>
                protected BossScript boss;

                /// <summary>
                /// Sets update up.
                /// </summary>
                /// <returns></returns>
                virtual public State Update() {

                    return null;
                }

                /// <summary>
                /// Referencing BossScript
                /// </summary>
                /// <param name="boss"></param>
                virtual public void OnStart(BossScript boss) {
                    this.boss = boss;
                }

                /// <summary>
                /// Tell when it is done
                /// </summary>
                virtual public void OnEnd() {

                }
            }

            //////////////////////////// Child Classes: 

            /// <summary>
            /// when the boss is doing nothing.
            /// </summary>
            public class Idle : State {

                /// <summary>
                /// how long the boss stays in idle.
                /// </summary>
                float idleTime = 5;

                /// <summary>
                /// Resets the values to their original value and bring in arguments
                /// </summary>
                /// <param name="time"></param>
                public Idle(float time) {
                    // re-declaring orginal idleTime
                    idleTime = time;
                    
                }

                /// <summary>
                /// Updates like every frame
                /// </summary>
                /// <returns></returns>
                public override State Update() {

                    // transitions: 
                    if (boss.health.health <= 0) // When the boss has no more health.
                        return new States.Death(); // Starts the death phase

                    if (boss.CanSeeThing(boss.attackTarget, true, boss.viewingDistance)) // When the boss can see the player
                        return new States.Pursuing(); // Start to pursue the player and kill it

                    // Patroling
                    idleTime -= Time.deltaTime; // counts idleTime down to start patroling 
                    // true when the boss can't see the player and idleTime is less than or equal to 0
                    if (!boss.CanSeeThing(boss.attackTarget, true, boss.viewingDistance) && idleTime <= 0) 
                        return new States.Patrolling(true); // Starts to patrol

                    return null; // returns and runs again
                }
            }

            /// <summary>
            /// When the Boss is pursuing the player
            /// </summary>
            public class Pursuing : State {
                public override State Update() {
                    // behavior:
                    boss.MoveTowardTarget(); // this chases down the player

                    // Transition:
                    if (boss.health.health <= 0) // if the boss has no health, it goes to the death phase
                        return new States.Death(); // death phase

                    if (!boss.CanSeeThing(boss.attackTarget, true, boss.viewingDistance)) // if the boss can no longer see the player
                        return new States.Idle(boss.timeToStopIdle); // goes back to idle mode


                    return null; // returns and runs again
                }
            }

            /// <summary>
            /// Patrols marked zones on the level when it is not attacking
            /// </summary>
            public class Patrolling : State {

                /// <summary>
                /// runs true to make boss move to designated patrol point
                /// </summary>
                bool runPatrolOnce = true;

                /// <summary>
                /// Makes runPatrolOnce true again when called
                /// </summary>
                /// <param name="patrolOnce"></param>
                public Patrolling(bool patrolOnce) {
                    runPatrolOnce = patrolOnce; // turns true
                }

                public override State Update() {

                    // behavior:
                    if (runPatrolOnce) { // if runPatrolOnce is true
                        boss.PatrolingPoints(); // goes to randomly selected patrol points
                        runPatrolOnce = false; // turning to false to run only once
                    }

                    // transition:
                    if (boss.health.health <= 0) // if the boss has no health
                        return new States.Death(); // death phase

                    if (!boss.nav.pathPending && boss.nav.remainingDistance <= 2f) //if the boss has reached his patroll point
                        return new States.Idle(boss.timeToStopIdle); // goes back to idle and send idleTimer value

                    if (boss.CanSeeThing(boss.attackTarget, false, boss.viewingDistance)) // if the boss can see the player
                        return new States.Pursuing(); // starts pursuing phase

                    return null; // returns and runs again
                }
            }

            /// <summary>
            /// When the boss has no health, this will run.
            /// </summary>
            public class Death : State {
                public override State Update() {
                    // Behavior:
                    boss.DeathEffect(); // Starts the death process
                    
                    return new States.Stunned(); // goes to Stunned to stop moving and 're-calling' DeathEffect
                }

            }

            /// <summary>
            /// Stops Boss from moving around and doing any action
            /// </summary>
            public class Stunned : State {
                
            }

        }

        // EnemyBasicController.States.State

        /// <summary>
        /// access the state pattern, maintain it, and make it function.
        /// </summary>
        private States.State state;

        /// <summary>
        /// controls the boss's navigation across the map
        /// </summary>
        private NavMeshAgent nav;

        /// <summary>
        /// The location of the player to attack at.
        /// </summary>
        public Transform attackTarget;
        
        /// <summary>
        /// Partrol points that are set to go to when patroling.
        /// </summary>
        public Transform[] patrolPoints;

        /// <summary>
        /// Which point the boss is patrolling
        /// </summary>
        private int pointPatrolling = 0;


        /// <summary>
        /// Distance the boss can see
        /// </summary>
        public float viewingDistance = 10;

        /// <summary>
        /// Angle the boss can see
        /// </summary>
        public float viewingAngle = 35;

        /// <summary>
        /// When the boss should stop at target
        /// </summary>
        public float stoppingDistance = 25;

        /// <summary>
        /// Range the boss is able to shot at target
        /// </summary>
        public float shootingRange = 15;

        /// <summary>
        /// When the boss should transition to patroling
        /// </summary>
        private float timeToStopIdle = 5;


        /// <summary>
        /// health of the boss
        /// </summary>
        private HealthScript health;

        /// <summary>
        /// Particle explosion when the boss dies
        /// </summary>
        public ParticleSystem deathParticleSystem;

        /// <summary>
        /// Get's player camera to switch to the boss when it dies
        /// </summary>
        public CameraTracking cameraSwitching;

        void Start() {
            nav = GetComponent<NavMeshAgent>(); // getting NavMeshAgent component
            health = GetComponent<HealthScript>(); // getting the HealthScript of the boss's health
        }

        void Update() {

            // if nothing is assigned to the state, then make the state go to the Idle() state
            if (state == null) SwitchState(new States.Idle(timeToStopIdle)); 
            //if (state == null) state = 

            if (state != null) SwitchState(state.Update()); // makes the state run it's update method
        }

        /// <summary>
        /// Move nav mesh agent (the boss) towards the player.
        /// </summary>
        void MoveTowardTarget() {
            if (attackTarget) nav.SetDestination(attackTarget.position); // sets point for boss to go to
            
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
        /// Checks to see if the boss can see the target
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="patrolVision"></param>
        /// <param name="shootingAndLooking"></param>
        /// <returns></returns>
        private bool CanSeeThing(Transform thing, bool patrolVision, float shootingAndLooking) {

            if (!thing) return false; // can't see anything

            Vector3 vToThing = thing.position - transform.position; // gets distance from the boss itself to the target (player)

            // checks the distance and to stop moving if the boss can't see the player
            if (vToThing.sqrMagnitude > shootingAndLooking * shootingAndLooking) {
                if (patrolVision) StopNavMovement();
                return false; // Too far away to see
            }

            // check the angle/area around the boss for a target
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // Stops boss at a certain distance at the target
            if (vToThing.sqrMagnitude < shootingAndLooking * (shootingAndLooking - stoppingDistance)) {
                StopNavMovement();
            } else
                ContinueNavMovement(); // if the boss is not close enough

            return true; // returns true
        }

        /// <summary>
        /// Stops boss from moving
        /// </summary>
        void StopNavMovement() {
            nav.updatePosition = false; // returns false on updatePosition
            nav.nextPosition = gameObject.transform.position; // sets point to where the boss is to stop it from moving
        }

        /// <summary>
        /// Makes boss continue moving
        /// </summary>
        void ContinueNavMovement() {
            nav.updatePosition = true; // returns true on updatePosition
        }

        /// <summary>
        /// Sets the random patrol point for the boss to go to.
        /// </summary>
        void PatrolingPoints() {

            // Got help understanding patrolling with Unity Documentation
            nav.updatePosition = true; // sets to true
            nav.destination = patrolPoints[pointPatrolling].position; // sets point to go to 
            pointPatrolling = (pointPatrolling + 1) % patrolPoints.Length; // assigns the next point to go to

        }

        /// <summary>
        /// Makes the death action and effects happen.
        /// </summary>
        void DeathEffect() {
            SoundEffectBoard.BossDeathSound(); // plays the boss death sound when the boss dies
            nav.enabled = false; // turns nav off to stop moving
            if (cameraSwitching) { // if cameraSwitching is assigned
                cameraSwitching.target = this.transform; // moves camera to the boss position
                cameraSwitching.smoothTransition = .3f; // sets the transition speed
            }
            // starts the particle explosion
            ParticleSystem sparks = Instantiate(deathParticleSystem, transform.position, Quaternion.identity);
        }
    }
}