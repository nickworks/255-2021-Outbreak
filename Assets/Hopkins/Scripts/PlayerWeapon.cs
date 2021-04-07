using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hopkins
{
    public class PlayerWeapon : MonoBehaviour
    {

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
                        // no ammo > cooldown:
                        if (weapon.roundsInClip <= 0) return new States.Cooldown(weapon.reloadTime);
                        // if ammo > shooting:
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
                    // behavior:
                    weapon.SpawnProjectile();
                    // transitions:
                    if (!Input.GetButton("Fire1")) return new States.Regular();
                    return null;
                }
            }
            public class Cooldown : State
            {
                /// <summary>
                /// how many seconds left in state
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

        public Projectile prefabProjectile;
        private States.State state;
        public int maxRoundsInClip = 15;
        private int roundsInClip = 15;
        /// <summary>
        /// # of bullets to spawn per second. used to calculate timing between bullets
        /// </summary>
        public float roundsPerSecond = 5;
        /// <summary>
        /// # of seconds until we can fire again.
        /// </summary>
        private float timerSpawnBullet = 0;

        public float reloadTime = 0;


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
            if (state != null) state.OnEnd();
            state = newState;
            state.OnStart(this);
        }


        void SpawnProjectile()
        {
            if (timerSpawnBullet > 0) return; // wait more
            if (roundsInClip <= 0) return; // 0 ammo

            Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            roundsInClip--;
            timerSpawnBullet = 1 / roundsPerSecond;
        }

    }
}