using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    /// <summary>
    /// this class handles the boss attack modes 
    /// </summary>
    public class BossAttack : MonoBehaviour
    {
        /// <summary>
        /// the state machine class
        /// </summary>
        static class States
        {
            public class State
            {
                /// <summary>
                /// to get access to variables outside 
                /// </summary>
                protected BossAttack attack;
                /// <summary>
                /// update is setup
                /// </summary>
                /// <returns></returns>
                virtual public State Update()
                {
                    return null;
                }
                /// <summary>
                /// referencing tghe BossAttack state 
                /// </summary>
                /// <param name="attack"></param>
                virtual public void OnStart(BossAttack attack)
                {
                    this.attack = attack;
                }
                /// <summary>
                /// when machine is done running 
                /// </summary>
                virtual public void OnEnd()
                {

                }
            }
            /// <summary>
            /// this is meant to be the regular attack state where player is not in range and does not attack
            /// </summary>
            public class Regular : State
            {
                public override State Update()
                {
                    if (attack.PlayerSeen(attack.PlayerTank, attack.viewingDis)) return new States.HeavyShot();/// player is seen and switch to heavy shot state

                    return null;
                }
            }
            /// <summary>
            /// this is the main shooting state for boss 
            /// </summary>
            public class HeavyShot : State
            {
                public override State Update()
                {
                    attack.HeavyShot();// runs the Heavy shot method 

                    if (!attack.PlayerSeen(attack.PlayerTank, attack.viewingDis)) return new States.Regular();// player is no longer in range, therefore stop shooting

                    return null;
                }

            }
            /// <summary>
            /// this is supposed to have the boss have a long reload state for the heavy shot
            /// </summary>
            public class Reload : State
            {
                float timetoReload = 7;// how long the reload takes 

                public Reload(float reload)
                {
                    timetoReload = reload; 
                }
            
                public override State Update()
                {
                   timetoReload -= Time.deltaTime;

                    if (timetoReload <= 0) return new States.Regular();

                    return null;
                
                }

            }
            /// <summary>
            /// was made for a flamethrower state, got the particle effect to work but not the acual state
            /// </summary>
            public class Flamethrower : State
            {

            }
            /// <summary>
            /// made for a third attack state but never got it to work
            /// </summary>
            public class Bombs : State
            {

            }

        }

        /// <summary>
        /// accesses the state machine pattern
        /// </summary>
        /// 

        private States.State state;
        /// <summary>
        /// reference to Projectile script
        /// </summary>
        /// 
        public Projectile HeavyShotBullet;

        /// <summary>
        /// refernce to where the bullet needs to spawn from
        /// </summary>
        public Transform bulletSpawn;

        /// <summary>
        /// the target it is attacking 
        /// </summary>
        public Transform PlayerTank;

        /// <summary>
        /// boss field of view
        /// </summary>
        public float viewingAng = 360;

        /// <summary>
        /// boss vision distance
        /// </summary>
        private float viewingDis = 40;

        /// <summary>
        /// how long the reload should takes
        /// </summary>
        public float ReloadTime = 7;

        /// <summary>
        /// how many shots the boss should have per second
        /// </summary>
        private float roundsPerSec = 5;

        private float HeavyShotTimer = 0;

        /// <summary>
        /// how many shots the boss should have in the level
        /// </summary>
        private int TotalHeavyShots = 75;

        /// <summary>
        /// reference to the rotation of the boss
        /// </summary>
        private Quaternion startingRot;

        void Start()
        {
            startingRot = transform.localRotation; // gets the local rotation of the boss
        }

        void Update()
        {
            if (state == null) SwitchState(new States.Regular());// go to regular state when no other state is assigned 

            if (state != null) SwitchState(state.Update());// run the update method

            if (HeavyShotTimer > 0) HeavyShotTimer -= Time.deltaTime;// amount of bullets it should fire

        }
        /// <summary>
        /// allows the state machine to switch between different states
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState)
        {
            if (newState == null) return;// do not switch 


            if (state != null) state.OnEnd();// when is previous state finished running
            state = newState;// change the states
            state.OnStart(this);
        }
        /// <summary>
        /// fires the heavshot
        /// </summary>
        void HeavyShot()
        {
            if (HeavyShotTimer > 0) return; // rate of fire

            Projectile HS = Instantiate(HeavyShotBullet, transform.position, Quaternion.identity); // spawn the heavhsot bullets
            HS.InitBullet(transform.forward * 35);// velocity of the projectile 

            TotalHeavyShots--; // should remove a bullet
            HeavyShotTimer = 1 / roundsPerSec;

        }

        private bool PlayerSeen(Transform thing, float visibleDis)
        {
            if (!thing) return false; // player is not visible and immmediately returns

            Vector3 vToThing = thing.position - transform.position;

            // checking the distance away from player
            if (vToThing.sqrMagnitude > visibleDis * visibleDis)
            {
                return false;
            }

            // checking it surrounding to see if player is withing its vision (360), then if not returning false
            if (Vector3.Angle(transform.forward, vToThing) > viewingAng) return false;

            return true;
        }

    }

}
