using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Jelsomeno
{
    /// <summary>
    /// this class tries to use a state pattern to handle the boss movement 
    /// </summary>
    public class Boss : MonoBehaviour
    {
        /// <summary>
        /// the state pattern class
        /// </summary>
        static class States
        {
            public class State
            {
                /// <summary>
                /// boss needs to get access to outside variables
                /// </summary>
                protected Boss boss;

                /// <summary>
                /// update is setup
                /// </summary>
                /// <returns></returns>
                virtual public State Update()
                {
                    return null;
                }
                
                /// <summary>
                /// references the boss script
                /// </summary>
                /// <param name="boss"></param>                
                virtual public void OnStart(Boss boss)
                {
                    this.boss = boss;
                }
                /// <summary>
                /// lets me know when it is done
                /// </summary>
                virtual public void OnEnd()
                {

                }
            }

            /// <summary>
            /// when the boss is in a idle type state and does not see the player
            /// </summary>
            public class Regular : State
            {
                /// <summary>
                /// how long the boss should stay in this mode
                /// </summary>
                float regularTime = 15;

                /// <summary>
                /// resets values
                /// </summary>
                /// <param name="time"></param>
                public Regular(float time)
                {
                    regularTime = time;
                }
                /// <summary>
                /// updating every frame
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // transistion//
                    if (boss.PlayerSeen(boss.PlayerTank, true, boss.viewingDis)) return new States.AttackPlayer(); // starting the attack phase once player is within range


                    return null;
                }
                
            }
            /// <summary>
            /// meant for a roaming state but never finished 
            /// </summary>
            public class Roaming : State
            {

            }

            /// <summary>
            /// this state will allow for the boss to see and move towards the player
            /// </summary>
            public class AttackPlayer : State
            {
                public override State Update()
                {
                    boss.MoveTowardsPlayer();/// uses move towards player

                    if (!boss.PlayerSeen(boss.PlayerTank, true, boss.viewingDis)) return new States.Regular(boss.timeToStop);// once player is out of range it goes back to regular state

                    return null;
                }
            }
            /// <summary>
            /// meant for death state but never finished 
            /// </summary>
            public class Death : State
            {

            }

        
        }


        /// <summary>
        /// accesses the state machine pattern
        /// </summary>
        private States.State state;

        /// <summary>
        /// references the nav mesh navisgation for the boss to move around the map
        /// </summary>
        private NavMeshAgent nav;

        /// <summary>
        /// references the player and its location
        /// </summary>
        public Transform PlayerTank;

        /// <summary>
        /// boss vision distance
        /// </summary>
        public float viewingDis = 10;

        /// <summary>
        /// boss field of view
        /// </summary>
        public float viewingAng = 35;

        /// <summary>
        /// how far away it should stop from target
        /// </summary>
        public float stopDis = 25;

        /// <summary>
        /// what range it needs to be into shoot
        /// </summary>
        public float InRange = 15;

        /// <summary>
        /// when the bost should transition from regular
        /// </summary>
        private float timeToStop = 5;




        // Start is called before the first frame update
        void Start()
        {
           nav = GetComponent<NavMeshAgent>();// get nav mesh component

        }

        // Update is called once per frame
        private void Update()
        {
            if (state == null) SwitchState(new States.Regular(stopDis));// go to regular state if nothing else is assigned

            if (state != null) SwitchState(state.Update()); // makes the state run it update


        }
        /// <summary>
        /// controls the nav mesh agent to move towards the player
        /// </summary>
        void MoveTowardsPlayer()
        {
            if (PlayerTank != null) nav.SetDestination(PlayerTank.position);// position of player so boss knows where to go
        }
        /// <summary>
        /// allows the state machine to switch between different states
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // do not switch


            if (state != null) state.OnEnd(); // when is previous state finished running
            state = newState; // change the states
            state.OnStart(this);
        }
        /// <summary>
        /// whether or not the player can be seen by the boss 
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="vision"></param>
        /// <param name="shooting"></param>
        /// <returns></returns>
        private bool PlayerSeen(Transform thing, bool vision, float shooting)
        {
            if (!thing) return false; // player is not visible and immmediately returns

            Vector3 vToThing = thing.position - transform.position;// distance from boss to players

            
            if (vToThing.sqrMagnitude > shooting * shooting) // stop moving if the boss can not see player and keeps them a certain distancse away from player
            {
                if (vision) stopMoving();

                return false;
            }

            if (Vector3.Angle(transform.forward, vToThing) > viewingAng) return false; // not close enough to player


            return true;// returning true 
        }
        /// <summary>
        ///  stops boss movement 
        /// </summary>
        void stopMoving()
        {
            nav.updatePosition = false;
            nav.nextPosition = gameObject.transform.position;
        }

        /// <summary>
        ///  keeps the boss moving
        /// </summary>
        void ContinueMovement()
        {
            nav.updatePosition = true;
        }
    }

}
