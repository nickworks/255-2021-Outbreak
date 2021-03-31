using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foster
{
    public class PlayerWeapon : MonoBehaviour
    {
        public Projectile prefabProjectile;

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
                    //behaviour

                    //transitions
                    if (Input.GetButton("Fire1"))
                    {
                        if (weapon.roundsInClip <= 0) return new States.Cooldown(weapon.reloadTime);

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
                    //Behaviour
                    weapon.SpawnProjectile();


                    //transition
                    if (!Input.GetButton("Fire1")) return new States.Regular();

                    return null;
                }
            }
            public class Cooldown: State
            {
                private float timeLeft = 3;

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
                    weapon.roundsInClip = weapon.maxRoundsInClips;  
                }

            }
        }

        private States.State state;

        public int maxRoundsInClips = 8;
        private int roundsInClip = 8;
        public float reloadTime = 1;
        /// <summary>
        /// How many bullets to spawn per second. We use this calculate the timing between bullets.
        /// </summary>
        public float roundsPerSecond = 5;
        /// <summary>
        /// how many seonds till we can fire again
        /// </summary>
        private float timerSpawnBullet = 0;

        void Start()
        {

        }


        void Update()
        {
           if(timerSpawnBullet >0) timerSpawnBullet -= Time.deltaTime;
            if (state == null) SwitchState(new States.Regular());

            //call stat.update()
            //switch to the return state
            if (state != null) SwitchState(state.Update());

            
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;
            if (state != null) state.OnEnd();

            state = newState;
            state.OnStart(this);
        }

        void SpawnProjectile()
        {
            if (timerSpawnBullet > 0) return;//we need to wait longer
            if (roundsInClip <= 0) return;//no ammo

            Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            roundsInClip--;
            timerSpawnBullet = 1 / roundsPerSecond;
        }
    }
}