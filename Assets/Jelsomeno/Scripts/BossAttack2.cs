using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    public class BossAttack2 : MonoBehaviour
    {
        static class States
        {
            public class State
            {

                /// <summary>
                /// To get access outside of this child class, boss is needed to access outside variables.
                /// </summary>
                protected BossAttack2 bossAttack;

                /// <summary>
                /// Sets update up.
                /// </summary>
                /// <returns></returns>
                virtual public State Update()
                {

                    return null;
                }

                /// <summary>
                /// Referencing BossAttackState
                /// </summary>
                /// <param name="bossAttack"></param>
                virtual public void OnStart(BossAttack2 bossAttack)
                {
                    this.bossAttack = bossAttack;
                }

                /// <summary>
                /// Tell when it is done
                /// </summary>
                virtual public void OnEnd()
                {

                }
            }

            //////////////////////////// Child Classes: 

            /// <summary>
            /// State when the boss has no targets to attack
            /// </summary>
            public class Idle : State
            {
                public override State Update()
                {

                    // Transitions:
                    if (bossAttack.healthAmt.health <= 0) // if the boss's health is at 0 or less, it will stop everything.
                        return null;

                    // if the boss turret can see the player and has bullets in clip
                    if (bossAttack.bulletAmountInClip > 0 && bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance))
                        return new States.HeavyShot(); // goes to MiniGunAttack() state

                    if (bossAttack.viewingDistance <= 5) return new States.Flamethrower();


                    return null;
                }

            }


            /// <summary>
            /// State that fires the minigun
            /// </summary>
            public class HeavyShot : State
            {
                public override State Update()
                {
                    // behaviour
                    bossAttack.MachineGun(); // run the machine gun method
                    bossAttack.TurnTowardsTarget(); // runs method to turn towards the target

                    // transition
                    if (bossAttack.healthAmt.health <= 0) // if health is a 0 or less (When boss dies)
                        return new States.Idle(); // goes to Idle() state

                    // if the boss turret can't see the enemy
                    if (!bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance)) return new States.Idle(); // goes to Idle() state

                    // if the boss has no bullets
                    if (bossAttack.bulletAmountInClip <= 0) return new States.Reload(bossAttack.reloadingTime); // goes to Reload() state

                    if (bossAttack.viewingDistance <= 5) return new States.Flamethrower();

                    return null;
                }
            }

            /// <summary>
            /// State for boss to reload minigun
            /// </summary>
            public class Reload : State
            {

                float reloadTime = 5; // time to reload

                public Reload(float reload)
                {
                    reloadTime = reload; // re-sets reload time
                }

                public override State Update()
                {
                    // behaviour
                    reloadTime -= Time.deltaTime; // counts down the reload time

                    if (reloadTime <= 0) return new States.Idle(); // if reload time is at or below 0, goes to Idle() state

                    return null;
                }

                // At the end of the state
                public override void OnEnd()
                {
                    bossAttack.bulletAmountInClip = bossAttack.maxRoundsToHave; // refills bullets
                }
            }

            /// <summary>
            /// State for Homing Missiles to spawn
            /// </summary>
            public class Flamethrower : State
            {

                public override State Update()
                {

 

                    return new States.HeavyShot(); // goes to Idle() state
                }
            }
        }

        /// <summary>
        /// access the state pattern, maintain it, and make it function.
        /// </summary>
        private States.State state;

        /// <summary>
        /// Prefab for the boss's bullets
        /// </summary>
        public EnemyProjectile prefabBullets;


        /// <summary>
        /// amount of bullets the boss can fire
        /// </summary>
        private int bulletAmountInClip = 50;

        /// <summary>
        /// Max bullets it can set
        /// </summary>
        public int maxRoundsToHave = 50;

        /// <summary>
        /// Time it takes to reload
        /// </summary>
        public float reloadingTime = 0;


        /// <summary>
        /// Left muzzle to fire bullets from
        /// </summary>
        public Transform bulletSpawn;



        /// <summary>
        /// Rounds per second to fire
        /// </summary>
        public float roundPerSec = 20;

        /// <summary>
        /// Amount of bullet to fire per second
        /// </summary>
        private float bulletAmountTime = 0;


        /// <summary>
        /// Health of the boss
        /// </summary>
        public HealthSystem healthAmt;


        /// <summary>
        /// Boss turret viewing angle
        /// </summary>
        public float viewingAngle = 90;

        /// <summary>
        /// Boss turret viewing distance
        /// </summary>
        public float viewingDistance = 20;

        /// <summary>
        /// target to shoot at in viewing
        /// </summary>
        public Transform player;

        /// <summary>
        /// The start rotation
        /// </summary>
        private Quaternion startingRotation;

        [Header("Rotation Lock")]

        /// <summary>
        /// Lock X rotation
        /// </summary>
        public bool lockRotationX;

        /// <summary>
        /// Lock Y rotation
        /// </summary>
        public bool lockRotationY;

        /// <summary>
        /// Lock Z rotation
        /// </summary>
        public bool lockRotationZ;


        void Start()
        {
            // Getting components
            startingRotation = transform.localRotation; // gets the local rotation to rotate back to when there is no target
            healthAmt = GetComponentInParent<HealthSystem>(); // gets HealthScript for health information
        }

        private void Update()
        {

            // Makes the script not run if boss health is 0 or below:
            if (healthAmt.health <= 0)
            {
                return;
            }

            // if nothing is assigned to the state, then make the state go to the Regular() state
            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update()); // makes the state run it's update method

            if (bulletAmountTime > 0) bulletAmountTime -= Time.deltaTime; // amount of bullets it would fire


            // missile spawn it TRUE
            // FALSE when fired
            // Begins count down
            // When count down is at 0, spawns missle and ready to fire
        }

 


        /// <summary>
        /// Makes the state swtich to a different state
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);
        }

        /// <summary>
        /// Turns turret towards the target
        /// </summary>
        private void TurnTowardsTarget()
        {

            if (player)
            { // if player is set as a target
                Vector3 disToTarget = player.position - transform.position; // Gets distance

                Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up); // Gets target rotation

                Vector3 euler1 = transform.localEulerAngles; // get local angles BEFORE rotation
                Quaternion prevRot = transform.rotation; // 
                transform.rotation = targetRotation; // Set Rotation
                Vector3 euler2 = transform.localEulerAngles; // get local angles AFTER rotation

                if (lockRotationX) euler2.x = euler1.x; //revert x to previous value;
                if (lockRotationY) euler2.y = euler1.y; //revert y to previous value;
                if (lockRotationZ) euler2.z = euler1.z; //revert z to previous value;

                transform.rotation = prevRot; // This objects rotation turns into the prevRot

                transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), .5f); // slides to rotation
            }
            else
            {
                // figure out bone rotation, no target:

                transform.localRotation = AnimMath.Slide(transform.localRotation, startingRotation, .05f);
            }
        }

        /// <summary>
        /// Fires the machine gun to shoot at target
        /// </summary>
        void MachineGun()
        {
            if (bulletAmountTime > 0) return; // rate of fire manager

            EnemyProjectile Bullets = Instantiate(prefabBullets, bulletSpawn.position, bulletSpawn.transform.rotation); // spawns bullet
            Bullets.InitBullet(transform.forward * 30); // sets velocity of the projectile

            bulletAmountInClip--; // removes a bullet
            bulletAmountTime = 1 / roundPerSec; // cause the rate of fire to happen
        }

        void Flamethrower()
        {

        }


        /// <summary>
        /// If the turret can see the target
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="viewingDis"></param>
        /// <returns></returns>
        private bool CanSeeThing(Transform thing, float viewingDis)
        {

            if (!thing) return false; // uh... error

            Vector3 vToThing = thing.position - transform.position; // distance from boss to target

            // check distance
            if (vToThing.sqrMagnitude > viewingDis * viewingDis)
            { // can't see target
                return false; // Too far away to see...
            }

            // check direction
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // TODO: Check occulusion

            return true;
        }
        
    }
}
