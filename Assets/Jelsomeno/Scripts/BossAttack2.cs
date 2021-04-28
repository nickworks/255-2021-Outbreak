using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    /// <summary>
    /// this class is meant to handle the bosses attacking modes using the state pattern
    /// </summary>
    public class BossAttack2 : MonoBehaviour
    {
        /// <summary>
        /// state pattern class
        /// </summary>
        static class States
        {
            public class State
            {

                /// <summary>
                /// boss is needs to get access to the outside variables.
                /// </summary>
                protected BossAttack2 bossAttack;

                /// <summary>
                /// Setting the update
                /// </summary>
                /// <returns></returns>
                virtual public State Update()
                {

                    return null;
                }

                /// <summary>
                /// References the BossAttack2
                /// </summary>
                /// <param name="bossAttack"></param>
                virtual public void OnStart(BossAttack2 bossAttack)
                {
                    this.bossAttack = bossAttack;
                }

                /// <summary>
                /// it is done
                /// </summary>
                virtual public void OnEnd()
                {

                }
            }

            /////////// Child Classes: 

            /// <summary>
            /// boss has nothing to attack
            /// </summary>
            public class Idle : State
            {
                public override State Update()
                {

                    // Transition to:
                    if (bossAttack.healthAmt.health <= 0) // the bosses health is gone
                        return null;

                    // the boss sees the player and has ammo
                    if (bossAttack.bulletAmount > 0 && bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance))
                        return new States.HeavyShot(); // go to main attack mode

                    if (bossAttack.viewingDistance <= 5) return new States.Flamethrower(); // meant to switch to flamethrower once player got into a certain range 


                    return null;
                }

            }


            /// <summary>
            /// shoots the main weapon against the player
            /// </summary>
            public class HeavyShot : State
            {
                public override State Update()
                {
                    // behaviour
                    bossAttack.TankShoot(); // use the TankShoot method
                    bossAttack.TurnToPlayer(); // points at the player by using this method

                    // transition to :

                    if (bossAttack.healthAmt.health <= 0) // boss dies when it loses all its health
                        return new States.Idle(); // goes back to the idle state

                    // boss can not see the player
                    if (!bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance)) return new States.Idle(); // goes back to the idle state

                    // boss has not ammo ready
                    if (bossAttack.bulletAmount <= 0) return new States.Reload(bossAttack.reloadingTime); // reload 

                    if (bossAttack.viewingDistance <= 5) return new States.Flamethrower(); // meant to switch to flamethrower once player got into a certain range 

                    return null;
                }
            }

            /// <summary>
            /// reload the main weapon for the gun
            /// </summary>
            public class Reload : State
            {

                float reloadTime = 5; // how long it takes to reload

                public Reload(float reload)
                {
                    reloadTime = reload; // timer is reset
                }

                public override State Update()
                {

                    reloadTime -= Time.deltaTime; // reload timer

                    if (reloadTime <= 0) return new States.Idle(); // goes to idle well reloading

                    return null;
                }

                // end of this state
                public override void OnEnd()
                {
                    bossAttack.bulletAmount = bossAttack.maxRounds; // reload back to max
                }
            }

            /// <summary>
            /// this was meant for flamethrower but was having probelems getting a particle system to do damage
            /// </summary>
            public class Flamethrower : State
            {

                public override State Update()
                {

                    return new States.HeavyShot(); // goe back to the Heavy Shot
                }
            }
        }

        /// <summary>
        /// accesses the state pattern and makes it function.
        /// </summary>
        private States.State state;

        /// <summary>
        /// the object that will act as the bullets for the boss
        /// </summary>
        public EnemyProjectile prefabBullets;


        /// <summary>
        /// how many heavy shots the boss can take
        /// </summary>
        private int bulletAmount = 50;

        /// <summary>
        /// Max amount of shots for the boss to have
        /// </summary>
        public int maxRounds = 50;

        /// <summary>
        /// how long it will take to reload
        /// </summary>
        public float reloadingTime = 0;


        /// <summary>
        /// where to spawn the heavy shot from
        /// </summary>
        public Transform bulletSpawn;

        /// <summary>
        /// Rounds per second 
        /// </summary>
        public float roundPerSec = 20;

        /// <summary>
        ///  bullets to shoot per second
        /// </summary>
        private float bulletAmountTime = 0;


        /// <summary>
        /// total health of the boss
        /// </summary>
        public HealthSystem healthAmt;


        /// <summary>
        /// field of the view for the heavy shot, same as in the boss2 script
        /// </summary>
        public float viewingAngle = 90;

        /// <summary>
        /// viewing distance for the boss to attack the player, same as in the boss2 script
        /// </summary>
        public float viewingDistance = 20;

        /// <summary>
        /// gets reference to the player which is the target of the boss
        /// </summary>
        public Transform player;

        /// <summary>
        /// start rotating
        /// </summary>
        private Quaternion startingRotation;

        [Header("Rotation Lock")]

        /// <summary>
        /// Locks the X-rotation
        /// </summary>
        public bool lockRotationX;

        /// <summary>
        /// Locks the Y-rotation
        /// </summary>
        public bool lockRotationY;

        /// <summary>
        /// Locks the Z-rotation
        /// </summary>
        public bool lockRotationZ;


        void Start()
        {
            startingRotation = transform.localRotation; // gets the local rotation
            healthAmt = GetComponentInParent<HealthSystem>(); // gets a reference to the HealthSystem script at the start
        }

        private void Update()
        {

            // Makes the script not run if boss is dead
            if (healthAmt.health <= 0)
            {
                return;
            }

            if (state == null) SwitchState(new States.Idle()); // go to the idle state when no other state can be assigned

            if (state != null) SwitchState(state.Update()); // run the state update method

            if (bulletAmountTime > 0) bulletAmountTime -= Time.deltaTime; // amount of heavyshots the tank can shoot

        }

        /// <summary>
        /// Makes the state swtich to a different state
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // don't switch 

            if (state != null) state.OnEnd(); // when it is done tell other states
            state = newState; 
            state.OnStart(this);
        }

        /// <summary>
        /// Turns the barrel of the tank at the player
        /// </summary>
        private void TurnToPlayer()
        {

            if (player) // player is the target
            { 
                Vector3 disToTarget = player.position - transform.position; // how far away is the player

                Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up); // how the player is rotated
                Vector3 euler1 = transform.localEulerAngles; // get local angles 
                Quaternion prevRot = transform.rotation; 
                transform.rotation = targetRotation; // Set Rotation
                Vector3 euler2 = transform.localEulerAngles; // get local angles again

                if (lockRotationX) euler2.x = euler1.x; //revert to  previous value;
                if (lockRotationY) euler2.y = euler1.y; //revert to previous value;
                if (lockRotationZ) euler2.z = euler1.z; //revert to previous value;

                transform.rotation = prevRot; // go back to previous Rotaion

                transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), .5f); // slides into a smoother rotation
            }
            else
            {

                transform.localRotation = AnimMath.Slide(transform.localRotation, startingRotation, .05f);
            }
        }

        /// <summary>
        /// tank shoots its main weapon
        /// </summary>
        void TankShoot()
        {
            if (bulletAmountTime > 0) return; // how fast the tank shoots

            EnemyProjectile Bullets = Instantiate(prefabBullets, bulletSpawn.position, bulletSpawn.transform.rotation); // spawns bullet object
            Bullets.InitBullet(transform.forward * 30); // speed of object

            bulletAmount--; // removes a bullet from the tanks current ammo count
            bulletAmountTime = 1 / roundPerSec; // causes the rate of fire
        }


        /// <summary>
        /// boss can see the player/its enemy
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="viewingDis"></param>
        /// <returns></returns>
        private bool CanSeeThing(Transform thing, float viewingDis)
        {

            if (!thing) return false; // return

            Vector3 vToThing = thing.position - transform.position; // how far the player is from the boss


            if (vToThing.sqrMagnitude > viewingDis * viewingDis)
            { 
                return false; // player is to far so return
            }

            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // player is out of field of view

            return true;
        }
        
    }
}
