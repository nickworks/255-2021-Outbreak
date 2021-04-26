using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Jelsomeno
{

    public class Boss2 : MonoBehaviour
    {

        static class States
        {
            public class State
            {


                protected Boss2 boss2;


                virtual public State Update()
                {

                    return null;
                }


                virtual public void OnStart(Boss2 boss2)
                {
                    this.boss2 = boss2;
                }

                virtual public void OnEnd()
                {

                }
            }


            public class Idle : State
            {

                float idleTime = 5;

                public Idle(float time)
                {
                    
                    idleTime = time;

                }


                public override State Update()
                {

                    if (boss2.health.health <= 0) // When the boss has no more health.
                        return new States.Death(); // Starts the death phase

                    if (boss2.CanSeePlayer(boss2.attackTarget, true, boss2.viewingDistance)) // When the boss can see the player
                        return new States.MoveToPlayer(); // Start to pursue the player and kill it

                    // Patroling
                    idleTime -= Time.deltaTime; // counts idleTime down to start patroling 
                    // true when the boss can't see the player and idleTime is less than or equal to 0
                    if (!boss2.CanSeePlayer(boss2.attackTarget, true, boss2.viewingDistance) && idleTime <= 0)
                        return new States.Roaming(true); // Starts to patrol

                    return null; // returns and runs again
                }
            }


            public class MoveToPlayer : State
            {
                public override State Update()
                {
                    
                    boss2.MoveTowardPlayer();

                    if (boss2.health.health <= 0) // if the boss has no health, it goes to the death phase
                        return new States.Death(); // death phase

                    if (!boss2.CanSeePlayer(boss2.attackTarget, true, boss2.viewingDistance)) // if the boss can no longer see the player

                        return new States.Idle(boss2.timeToStopIdle); // goes back to idle mode


                    return null; 
                }
            }



            public class Roaming : State
            {

                bool runOnce = true;

                public Roaming(bool doOnce)
                {
                    runOnce = doOnce; 
                }

                public override State Update()
                {

                    // behavior:
                    if (runOnce)
                    { // if runPatrolOnce is true
                        boss2.RoamingAreas(); // goes to randomly selected patrol points
                        runOnce = false; // turning to false to run only once
                    }

                    if (boss2.health.health <= 0) // if the boss has no health
                        return new States.Death(); // death phase

                    if (!boss2.nav.pathPending && boss2.nav.remainingDistance <= 2f) //if the boss has reached his patroll point

                        return new States.Idle(boss2.timeToStopIdle); // goes back to idle and send idleTimer value

                    if (boss2.CanSeePlayer(boss2.attackTarget, false, boss2.viewingDistance)) // if the boss can see the player

                        return new States.MoveToPlayer(); // starts pursuing phase

                    return null; // returns and runs again
                }
            }


            public class Death : State
            {
                public override State Update()
                {
                    // Behavior:
                    boss2.AfterDeath();

                    return null;
                }

            }

        }

   
        private States.State state;


        private NavMeshAgent nav;


        public Transform attackTarget;

        public Transform[] RoamTo;

        private int pointPatrolling = 0;

        private HealthSystem health;


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
        /// Particle explosion when the boss dies
        /// </summary>
        public ParticleSystem explosion;

        /// <summary>
        /// Get's player camera to switch to the boss when it dies
        /// </summary>
        public CameraTracking cameraSwitching;

        public Transform[] PatrolPoints { get => RoamTo; set => RoamTo = value; }

        void Start()
        {
            nav = GetComponent<NavMeshAgent>(); // getting NavMeshAgent component
            health = GetComponent<HealthSystem>();
        }

        void Update()
        {

            // if nothing is assigned to the state, then make the state go to the Idle() state
            if (state == null) SwitchState(new States.Idle(timeToStopIdle));
            //if (state == null) state = 

            if (state != null) SwitchState(state.Update()); // makes the state run it's update method
        }

        /// <summary>
        /// Move nav mesh agent (the boss) towards the player.
        /// </summary>
        void MoveTowardPlayer()
        {
            if (attackTarget) nav.SetDestination(attackTarget.position); // sets point for boss to go to

        }

        /// <summary>
        /// Makes the state swtich to a different state
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState)
        {
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
        private bool CanSeePlayer(Transform thing, bool patrolVision, float shootingAndLooking)
        {

            if (!thing) return false; // can't see anything

            Vector3 vToThing = thing.position - transform.position; // gets distance from the boss itself to the target (player)

            // checks the distance and to stop moving if the boss can't see the player
            if (vToThing.sqrMagnitude > shootingAndLooking * shootingAndLooking)
            {
                if (patrolVision) StopNavMovement();
                return false; // Too far away to see
            }

            // check the angle/area around the boss for a target
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // Stops boss at a certain distance at the target
            if (vToThing.sqrMagnitude < shootingAndLooking * (shootingAndLooking - stoppingDistance))
            {
                StopNavMovement();
            }
            else
                ContinueNavMovement(); // if the boss is not close enough

            return true; // returns true
        }

        /// <summary>
        /// Stops boss from moving
        /// </summary>
        void StopNavMovement()
        {
            nav.updatePosition = false; // returns false on updatePosition
            nav.nextPosition = gameObject.transform.position; // sets point to where the boss is to stop it from moving
        }

        /// <summary>
        /// Makes boss continue moving
        /// </summary>
        void ContinueNavMovement()
        {
            nav.updatePosition = true; // returns true on updatePosition
        }

        /// <summary>
        /// Sets the random patrol point for the boss to go to.
        /// </summary>
        void RoamingAreas()
        {

            // Got help understanding patrolling with Unity Documentation
            nav.updatePosition = true; // sets to true
            nav.destination = PatrolPoints[pointPatrolling].position; // sets point to go to 
            pointPatrolling = (pointPatrolling + 1) % PatrolPoints.Length; // assigns the next point to go to

        }

        /// <summary>
        /// Makes the death action and effects happen.
        /// </summary>
        void AfterDeath()
        {
            Destroy(this.gameObject);
        }
    }
}

