using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class PlayerWeapon : MonoBehaviour
    {
        static class States
        {
            public class State {

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
            public class Regular : State {
                public override State Update()
                {
                    // behavior:

                    // transitions:
                    if (Input.GetButton("Fire1"))
                    {
                        // If no ammo in clip
                        if (weapon.roundsInClip <= 0) return new States.Reloading(weapon.reloadTime);

                        // If ammo, return to shoot.
                        return new States.Attacking();
                    }

                    if (Input.GetButton("Reload")) return new States.Reloading(weapon.reloadTime);

                    return null;
                }
                
            }
            public class Attacking : State {
                public override State Update()
                {
                    // behavior:
                    weapon.SpawnProjectile();

                    // transition:
                    if (!Input.GetButton("Fire1")) return new States.Regular();

                    return null;
                }
            }
            public class Reloading : State 
            {
                // How long until reloading is over.
                float timeLeft = 3;

                public Reloading(float time)
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

        // Inheritance
        // encapsulation
        // Polymorphism

        public Projectile prefabProjectile;
        private States.State state;

        public int maxRoundsInClip = 8;

        private int roundsInClip = 8;

        // Bullets to spawn per second. Calc delay between bullets.
        public float roundsPerSecond = 5;

        // Seconds until player can fire again.
        private float bulletCooldown = 0;

        public float reloadTime = 1;

        void Update()
        {
            if (bulletCooldown > 0) bulletCooldown -= Time.deltaTime;

            // If state doesn't exist, set it up.
            if (state == null) SwitchState(new States.Regular());

            // Call state.update, if it returns a state, switch to the returned state.
            if (state != null) SwitchState(state.Update());
        }

        void SwitchState(States.State newState)
        {
            // If null is passed in, return nothing.
            if (newState == null) return;

            // If we are in a state, call it's onEnd.
            if (state != null) state.OnEnd();

            // Switch the state
            state = newState;

            // Call the new state's start function.
            state.OnStart(this);
        }

        void SpawnProjectile()
        {
            if (bulletCooldown > 0) return; // Need to wait longer to shoot.
            if (roundsInClip <= 0) return; // No ammo


            Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            roundsInClip--;
            bulletCooldown = 1 / roundsPerSecond;
        }
    }
}

