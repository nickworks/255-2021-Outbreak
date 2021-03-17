using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kortge{
    public class Weapon : MonoBehaviour
    {
        public enum WeaponState
        {
            Regular,
            Attacking,
            Cooldown
        }


        public static class States
        {
            public class State {

                protected Weapon weapon;

                virtual public State Update() {
                    if (Input.GetButton("Fire1"))
                    {
                        if (weapon.roundsInClip <= 0) return new States.Cooldown();
                        return new States.Attacking();
                    }
                    return null;
                }
                virtual public void OnStart(Weapon weapon)
                {
                    this.weapon = weapon;
                }
                virtual public void OnEnd()
                {

                }
            }
            public class Regular : State {
                virtual public State Update()
                {

                    // transitions:
                    if (!Input.GetButton("Fire1")) return new States.Attacking();

                    return null;
                }
            }
            public class Attacking : State { // Fix
                public override State Update()
                {
                    if (!Input.GetButton("Fire1")) return new States.Regular();
                    weapon.SpawnProjectile();
                    return new States.Cooldown();
                }
            }
            public class Cooldown : State {
                float timeLeft = 3;

                public override State Update()
                {
                    timeLeft -= Time.deltaTime;

                    if (timeLeft < 0) return new States.Regular();

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

        private int maxRoundsInClip = 8;
        private int roundsInClip = 8;
        public float roundsPerSecond = 5;
        /// <summary>
        /// How many seconds until we can fire again.
        /// </summary>
        private float timerSpawnBullt = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (state == null) SwitchState(new States.Regular());

            if (state != null) {
                if (state != null) SwitchState(state.Update());
            };

            //if (timerSpawnBullt <= 0) 
        }

        void SwitchState(States.State newState)
        {
            if (newState == null || roundsInClip <=0) return;

            if (state != null) state.OnEnd();

            state = newState;

            state.OnStart(this);
        }

        void SpawnProjectile()
        {
            if (roundsInClip <= 0) return; // no ammo!
            Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            roundsInClip--;
            timerSpawnBullt = 1 / roundsPerSecond;
        }
    }
}