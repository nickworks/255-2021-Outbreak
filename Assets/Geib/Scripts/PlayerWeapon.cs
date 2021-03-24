using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geib
{
    public class PlayerWeapon : MonoBehaviour
    {
        /// <summary>
        /// This enumeration holds all the differnt player weapon states
        /// </summary>



        static class States
        {
            public class State
            {
                protected PlayerWeapon weapon;

                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(PlayerWeapon weapon)
                {
                    this.weapon = weapon;
                }
                virtual public void OnEnd()
                {

                }
            }
            public class Regular : State
            {
                public override State Update()
                {
                    // behavior:

                    // transitions:
                    if (Input.GetButton("Fire1"))
                    {
                        //reload if empty
                        if (weapon.roundsInClip <= 0) return new States.Cooldown(weapon.reloadTime);
                        
                        // else fire
                        return new States.Attacking();
                    }
                    if (Input.GetButton("Reload")) return new States.Cooldown(weapon.reloadTime);

                    return null;
                }
            }
            public class Attacking : State
            {
                public override State Update()
                {
                    //behavior:
                    weapon.SpawnProjectile();

                    //transitions:
                    if (!Input.GetButton("Fire1")) return new States.Regular();
                    
                    return null;
                }
            }
            public class Cooldown : State
            {
                /// <summary>
                /// How many seconds left until the cooldown state is over.
                /// </summary>
                float timeLeft = 3;

                public Cooldown(float time)
                {
                    timeLeft = time;
                }

                public override State Update()
                {
                    timeLeft -= Time.deltaTime;

                    if (timeLeft <= 0) return new States.Regular();

                    return null;
                }
                public override void OnEnd()
                {
                    weapon.roundsInClip = weapon.maxRoundsInClip;
                }
            }
        }

        // encapsulation
        // inheritance
        // polymorphism

        /// <summary>
        /// This varaible holds the projectile object that spawns when the player shoots
        /// </summary>
        public Projectile prefabProjectile;
        /// <summary>
        /// The current state
        /// </summary>
        private States.State state;
        /// <summary>
        /// Number of bullets in the gun
        /// </summary>
        private int roundsInClip = 8;
        /// <summary>
        /// The number of rounds you can fire in a second. We use this to calculate the delay between bullets
        /// </summary>
        public float roundsPerSecond = 5;
        /// <summary>
        /// how many seconds until we can fire again
        /// </summary>
        private float timerSpawnBullet = 0;

        public float reloadTime = 1;

        public int maxRoundsInClip = 8;

        // Update is called once per frame
        void Update()
        {
            if (timerSpawnBullet > 0) timerSpawnBullet -= Time.deltaTime;
            if (state == null) SwitchState(new States.Regular());

            // call state.update()
            // switch to the returned state
            if (state != null) SwitchState(state.Update());

        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;

            if(state != null) state.OnEnd();

            state = newState;

            state.OnStart(this);
        }

        void SpawnProjectile()
        {
            if (timerSpawnBullet > 0) return; // we need to wait longer to spawn bullets.
            if (roundsInClip <= 0) return; //No ammo


            Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            roundsInClip--;
            timerSpawnBullet = 1 / roundsPerSecond;
        }
    }
}