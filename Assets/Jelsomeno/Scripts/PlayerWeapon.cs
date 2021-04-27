using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
{
    /// <summary>
    /// players weapon state maching
    /// </summary>
    public class PlayerWeapon : MonoBehaviour
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
                protected PlayerWeapon weapon;

                /// <summary>
                /// update is setup
                /// </summary>
                /// <returns></returns>
                virtual public State Update()
                {
                    return null;
                }
                /// <summary>
                /// referencing tghe PlayerWeapon state machine
                /// </summary>
                /// <param name="attack"></param>
                virtual public void OnStart(PlayerWeapon weapon)
                {
                    this.weapon = weapon;
                }
                virtual public void OnEnd()
                {

                }
            }
            /// <summary>
            /// regular state of player
            /// </summary>
            public class Regular : State {
                public override State Update()
                {
                    //behavior 

                    // transitions:
                    
                    if (Input.GetButton("Fire1"))// left mouse click
                    {
                        if (weapon.roundsInClip <= 0) return new States.Reload(weapon.reloadTime); // go to attacking state

                        return new States.Attacking();
                    }

                    if (Input.GetButton("Reload")) return new States.Reload(weapon.reloadTime);// go to reload state

                    return null;

                }
            }
            /// <summary>
            /// enters the player into its attacking mode //// does not work
            /// </summary>
            public class Attacking : State {
                public override State Update()
                {
                    // behavior:

                    weapon.SpawnProjectile();// spawns bullet projectile

                    //transitions:
                    if (!Input.GetButton("Fire1")) return new States.Regular();// return back to regular state

                    return null;
                }
            }
            /// <summary>
            /// reloads player bullets
            /// </summary>
            public class Reload : State {


                float timeLeft = 3; // how long the reload takes 

                public Reload(float time)
                {
                    timeLeft = time; 
                }
                public override State Update()
                {
                    timeLeft -= Time.deltaTime;

                    if (timeLeft <= 0) return new States.Regular(); // goes back to the regular state 

                    return null;
                }
                public override void OnEnd()
                {
                    weapon.roundsInClip = 8; // how many rounds the player has
                }
            }
        }

        /// <summary>
        /// refernces the Projectile script
        /// </summary>
        public Projectile prefabProjectile;

        /// <summary>
        /// accesses the state machine pattern
        /// </summary>
        /// 
        private States.State state;

        /// <summary>
        /// how many bullets it has before needing to be reloaded 
        /// </summary>
        private int roundsInClip = 8;

        /// <summary>
        /// How many bullets to spawn per second. We use this to calculate the timing between bullets
        /// </summary>
        public float roundsPerSecond = 5;

        /// <summary>
        /// seconds until we can fire again
        /// </summary>
        private float timerSpawnBullet = 0;

        /// <summary>
        /// how long to reload
        /// </summary>
        public float reloadTime = 1;

        void Update()
        {
            if (timerSpawnBullet > 0) timerSpawnBullet -= Time.deltaTime;

            if (state == null) SwitchState(new States.Regular()); // go to regular state when no other state is assigned

            //call state.update
            //switch to the returned state
            if (state != null) SwitchState(state.Update());


            
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
        /// spawns the bullet
        /// </summary>
        void SpawnProjectile()
        {

            if (timerSpawnBullet > 0) return; // rate of fire
            if (roundsInClip <= 0) return; // no ammo

            Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity); // spawn the heavhsot bullets
            p.InitBullet(transform.forward * 20); // veolocity of projectile 

            roundsInClip--;// take a bullet away
            timerSpawnBullet = 1;

        }
    }
}
