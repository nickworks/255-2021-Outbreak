using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class uses a state pattern to control the player's attack action.
    /// </summary>
    public class PlayerWeapon : MonoBehaviour {


        /// <summary>
        /// The state pattern class
        /// </summary>
        static class States {
            public class State {

                /// <summary>
                /// To get access outside of this child class, boss is needed to access outside variables.
                /// </summary>
                protected PlayerWeapon weapon;

                /// <summary>
                /// Sets update up.
                /// </summary>
                /// <returns></returns>
                virtual public State Update() {
                    return null;
                }

                /// <summary>
                /// Referencing PlayerWeapon
                /// </summary>
                /// <param name="weapon"></param>
                virtual public void OnStart(PlayerWeapon weapon) {
                    this.weapon = weapon;
                }

                /// <summary>
                /// Tell when it is done
                /// </summary>
                virtual public void OnEnd() {

                }
            }

            /// <summary>
            /// The State when the player is not using any weapons
            /// </summary>
            public class Regular : State {
                public override State Update() {

                    // transitions:
                    if (weapon.playerHealth.health <= 0) return null; // if the player's health is a 0 or below, make the player not shoot anything.


                    if (Input.GetButtonDown("Fire2")) { // if the player presses right mouse button
                        if (weapon.rocketTimer <= 0) return new States.SpecialRocketLauncher(); // goes to SpecialRocketLauncher state
                    }

                    if (Input.GetButton("Fire1")) { // if the player press left mouse button

                        // if no ammo, go to cooldown:
                        if (weapon.roundsInClip <= 0) return new States.Cooldown(weapon.reloadTime);

                        // if ammo, go to shooting:
                        return new States.Attacking();
                    }
                    if (Input.GetButton("Reload")) return new States.Cooldown(weapon.reloadTime); // if the player presses r, it will go to cooldown state.

                    return null;
                }
            }

            /// <summary>
            /// State when the player is shooting main assult rifle
            /// </summary>
            public class Attacking : State {
                public override State Update() {
                    // behavior:
                    weapon.SpawnProjectile(); // spawns the projectiles when shooting

                    //print("Fire!");

                    // transition:
                    if (weapon.playerHealth.health <= 0) return new States.Regular(); // if the player's health is a 0 or below, make the player not shoot anything.


                    if (!Input.GetButton("Fire1")) return new States.Regular(); // if the player lets go of the left mouse button to stop shooting

                    return null;
                }
            }

            /// <summary>
            /// State for when the player needs to reload for more ammo
            /// </summary>
            public class Cooldown : State {

                /// <summary>
                /// How many seconds left in this state
                /// </summary>
                float timeLeft = 3;

                public Cooldown(float time) {
                    timeLeft = time; // re-setting value of the time left
                }

                public override State Update() {

                    timeLeft -= Time.deltaTime; // counts down

                    if (weapon.playerHealth.health <= 0) return new States.Regular(); // if the player's health is a 0 or below, make the player not shoot anything.
                    if (timeLeft <= 0) return new States.Regular(); // when the timeLeft is at 0 or below, it goes back into the Regular state

                    return null;
                }

                public override void OnEnd() {
                    weapon.roundsInClip = weapon.maxRoundsInClip; // puts rounds back into clip
                }

            }

            /// <summary>
            /// State to fire the rocket 
            /// </summary>
            public class SpecialRocketLauncher : State {
                public override State Update() {

                    weapon.SpawnRocket(); // goes to method to spawn the rocket

                    return new States.Regular(); // returns back to Regular state
                }
            }

        }

        // encapsulate
        // inheritance
        // polymorphism

        /// <summary>
        /// bullet prefab that will be spawned
        /// </summary>
        public Projectile prefabProjectile;

        /// <summary>
        /// access the state pattern, maintain it, and make it function.
        /// </summary>
        private States.State state;

        /// <summary>
        /// maximum amount of bullets that the rifle can shoot
        /// </summary>
        public int maxRoundsInClip = 20;

        /// <summary>
        /// Rounds currently in clip
        /// </summary>
        [HideInInspector] public int roundsInClip = 20;

        /// <summary>
        /// Rocket prefab that will be spawned once fired
        /// </summary>
        public RocketMechanic rocketPrefab;

        /// <summary>
        /// timer until the rocket can be fired again
        /// </summary>
        [HideInInspector] public float rocketTimer = 0;

        /// <summary>
        /// Sets the timer when the rocket it fired
        /// </summary>
        public float maxTimeForRocket = 8;

        /// <summary>
        /// How many bullets to spawn per second. We use this to calcalute the timing between bullets.
        /// </summary>
        public float roundsPerSecond = 5;

        /// <summary>
        /// How many seconds until we can fire again.
        /// </summary>
        private float timerSpawnBullet = 0;

        /// <summary>
        /// Time it takes to reload
        /// </summary>
        public float reloadTime = 0;

        /// <summary>
        /// muzzle where the rocket and bullets shoot out from
        /// </summary>
        public GameObject muzzle;

        /// <summary>
        /// The particle will spawn to resemple a muzzle flash on a gun.
        /// </summary>
        public ParticleSystem muzzleFlash;

        /// <summary>
        /// Getting the player health to check it
        /// </summary>
        private HealthScript playerHealth;

        void Start() {
            roundsInClip = maxRoundsInClip; // sets max rounds to the current rounds.
            playerHealth = GetComponent<HealthScript>(); // Gets the HealthScript component
        }

        void Update() {
           
            if (timerSpawnBullet > 0) timerSpawnBullet -= Time.deltaTime; // timer to spawn the bullet

            if (rocketTimer > 0) rocketTimer -= Time.deltaTime; // timer to be able to spawn rocket

            //// if nothing is assigned to the state, then make the state go to the Regular() state
            if (state == null) SwitchState(new States.Regular());

            // call state.update()
            // switch to the returned state
            // makes the state run it's update method
            if (state != null) 
                SwitchState(state.Update());
        }

        /// <summary>
        /// Makes the state swtich to a different state
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState) {

            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done

            state = newState; // swap states

            state.OnStart(this);

        }

        /// <summary>
        /// Spawns projectiles/bullets
        /// </summary>
        void SpawnProjectile() {

            if (timerSpawnBullet > 0) return; // we need to wait longer...
            if (roundsInClip <= 0) return; // no ammo

            SoundEffectBoard.PlayerShooting(); // players gunfire soundeffect

            // Spawns the bullets
            Projectile p = Instantiate(prefabProjectile, muzzle.transform.position, muzzle.transform.rotation);
            p.InitBullet(transform.forward * 30); // sets the velocity of the projectile

            Instantiate(muzzleFlash, muzzle.transform.position, muzzle.transform.rotation);

            roundsInClip--; // removes a bullet from roundInClip
            timerSpawnBullet = 1 / roundsPerSecond; // rate of fire
        }

        /// <summary>
        /// Spawns rocket
        /// </summary>
        void SpawnRocket() {
            if (rocketTimer > 0) return; // if the rocket timer is not below 0, doesn't run
            rocketTimer = maxTimeForRocket; // sets timer for it to count down
            SoundEffectBoard.RocketMissileSound(); // plays the rocket soundeffect

            // Spawns the rocket 
            RocketMechanic rocket = Instantiate(rocketPrefab, muzzle.transform.position, transform.rotation);
        }
    }
}