using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Jelsomeno
{
    /// <summary>
    /// This class acts as the boss movement using a state pattern to do so
    /// </summary>
    public class Boss2 : MonoBehaviour
    {
        /// <summary>
        /// state pattern class
        /// </summary>
        static class States
        {
            public class State
            {

                /// <summary>
                /// this is needed so the boss can get access to outside variables
                /// </summary>
                protected Boss2 boss2;

                /// <summary>
                /// setting the update up
                /// </summary>
                /// <returns></returns>
                virtual public State Update()
                {

                    return null;
                }

                /// <summary>
                /// references the Boss2 script
                /// </summary>
                /// <param name="boss2"></param>
                virtual public void OnStart(Boss2 boss2)
                {
                    this.boss2 = boss2;
                }

                /// <summary>
                /// when it is done
                /// </summary>
                virtual public void OnEnd()
                {

                }
            }

            /////////////// Child Classes
            

            /// <summary>
            /// simple idle state of boss, it often runs this state when in transitions to other states
            /// </summary>
            public class Idle : State
            {
                /// <summary>
                /// boss stays in idle state for 3 seconds
                /// </summary>
                float idleTime = 3;

                /// <summary>
                /// resets values to their original values
                /// </summary>
                /// <param name="time"></param>
                public Idle(float time)
                {
                    
                    idleTime = time; //goes back to original idel time

                }

                /// <summary>
                /// updates about every frame
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    //transition to:

                    if (boss2.health.health <= 0) // boss is out of health
                        return new States.Death(); // begins the Death state

                    if (boss2.CanSeePlayer(boss2.attackTarget, true, boss2.viewingDistance)) //if the boss can see the player
                        return new States.MoveToPlayer(); // goes to the MoveToPlayer state and starts towards the player and attacks it

                    
                    idleTime -= Time.deltaTime; // after idle state is over, start to roam
                    
                    if (!boss2.CanSeePlayer(boss2.attackTarget, true, boss2.viewingDistance) && idleTime <= 0) // when the player is no longer in viewing distance to attack
                        return new States.Roaming(true); // Start roaming to its points on the map

                    return null; // returns and runs again
                }
            }

            /// <summary>
            /// the boss will move to the player once it is in distance
            /// </summary>
            public class MoveToPlayer : State
            {
                public override State Update()
                {
                    
                    boss2.MoveTowardPlayer(); // moves towards the player

                    // transition to:

                    if (boss2.health.health <= 0) // boss is out of health
                        return new States.Death(); // goes to the Death state

                    if (!boss2.CanSeePlayer(boss2.attackTarget, true, boss2.viewingDistance)) // can no longer see player
                        return new States.Idle(boss2.timeToStopIdle);  // goes to the idle state

                    return null; // return and run again
                }
            }


            /// <summary>
            /// roams from point to point that are referenced on the map
            /// </summary>
            public class Roaming : State
            {
                /// <summary>
                /// makes boss move to points on map
                /// </summary>
                bool runOnce = true;

                /// <summary>
                /// keeps making runOnce true
                /// </summary>
                /// <param name="doOnce"></param>
                public Roaming(bool doOnce)
                {
                    runOnce = doOnce; //true
                }

                public override State Update()
                {

                    
                    if (runOnce) // runOnce = true
                    { 
                        boss2.RoamingAreas(); // randomly selects one of the points on the map
                        runOnce = false; // runOnce becomes false
                    }

                    // transition to:

                    if (boss2.health.health <= 0) // boss is out of health
                        return new States.Death(); // goes to the Death state

                    if (!boss2.nav.pathPending && boss2.nav.remainingDistance <= 2f) // boss reaches a point on the map
                        return new States.Idle(boss2.timeToStopIdle); // goes to idle State

                    if (boss2.CanSeePlayer(boss2.attackTarget, false, boss2.viewingDistance)) // the boss can see the player
                        return new States.MoveToPlayer(); // starts towards the player

                    return null; // return and runs again
                }
            }

            /// <summary>
            /// runs only when the boss has not health
            /// </summary>
            public class Death : State
            {
                public override State Update()
                {
                    
                    boss2.AfterDeath(); // start death 

                    return null;
                }

            }

        }


        /// <summary>
        /// acesses the state pattern and makes it function
        /// </summary>
        private States.State state;

        /// <summary>
        /// makes sure the boss can navigate around the map
        /// </summary>
        private NavMeshAgent nav;

        /// <summary>
        /// this gets the reference to the player so the boss know what it is attacking
        /// </summary>
        public Transform attackTarget;

        /// <summary>
        /// allows for me to set points for the boss to go to on the map
        /// </summary>
        public Transform[] RoamTo;

        /// <summary>
        /// what point on the map the boss is going to
        /// </summary>
        private int RoamingPoint = 0;

        /// <summary>
        /// reference to the health for the boss
        /// </summary>
        private HealthSystem health;

        /// <summary>
        /// how far the boss can see and the distance the player needs to be within to be seen
        /// </summary>
        public float viewingDistance = 10;

        /// <summary>
        /// Angle the boss can view within, basically its field of view
        /// </summary>
        public float viewingAngle = 35;

        /// <summary>
        /// the boss will keep a certain distance away from the player
        /// </summary>
        public float stoppingDistance = 25;

        /// <summary>
        /// boss transitions out of idle, usually to Roaming
        /// </summary>
        private float timeToStopIdle = 3;


        public Transform[] RoamPoints { get => RoamTo; set => RoamTo = value; }

        void Start()
        {
            nav = GetComponent<NavMeshAgent>(); // gets NavMeshAgent component
            health = GetComponent<HealthSystem>(); // gets reference to the health scripts once the game is started
        }

        void Update()
        {
  
            if (state == null) SwitchState(new States.Idle(timeToStopIdle)); // when no state is assigned just run the Idle state
           
            if (state != null) SwitchState(state.Update()); // runs the states pattern update
        }

        /// <summary>
        /// nav mesh helps move boss towards the player
        /// </summary>
        void MoveTowardPlayer()
        {
            if (attackTarget) nav.SetDestination(attackTarget.position); // gets the destination of the player and it makes it the point for the boss to go to

        }

        /// <summary>
        /// the state swtiches to a different state
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // don't switch 

            if (state != null) state.OnEnd(); // previous state is done
            state = newState; // change the states
            state.OnStart(this);
        }

        /// <summary>
        /// this checks to see if the player can be seen by the boss
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="patrolVision"></param>
        /// <param name="shootingAndLooking"></param>
        /// <returns></returns>
        private bool CanSeePlayer(Transform thing, bool patrolVision, float shootingAndLooking)
        {

            if (!thing) return false; // boss does not see player

            Vector3 vToThing = thing.position - transform.position; // distance between the boss and player

            //  stop moving if the boss can't see the player
            if (vToThing.sqrMagnitude > shootingAndLooking * shootingAndLooking)
            {
                if (patrolVision) StopNavMovement();
                return false; 
            }

            // check the angle/area for the boss
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of field of view

            // at a certain distance away from the player then stop moving the boss
            if (vToThing.sqrMagnitude < shootingAndLooking * (shootingAndLooking - stoppingDistance))
            {
                StopNavMovement();
            }
            else
                ContinueNavMovement(); //  boss is not close enough then it can keep moving

            return true; // returns true
        }

        /// <summary>
        /// keeps the boss from moving 
        /// </summary>
        void StopNavMovement()
        {
            nav.updatePosition = false; // updatePosition is fales
            nav.nextPosition = gameObject.transform.position; // point at which the boss can no longer move
        }

        /// <summary>
        /// boss continues moving
        /// </summary>
        void ContinueNavMovement()
        {
            nav.updatePosition = true; // updatePosition is true
        }

        /// <summary>
        /// this is to set up the roaming points for the boss
        /// </summary>
        void RoamingAreas()
        {
            nav.updatePosition = true; // updatePosition is true
            nav.destination = RoamPoints[RoamingPoint].position; // what point to go to
            RoamingPoint = (RoamingPoint + 1) % RoamPoints.Length; // chooses the next point

        }

        /// <summary>
        ///destroys the boss
        /// </summary>
        void AfterDeath()
        {
            Destroy(this.gameObject);
        }
    }
}

