using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// Class for the Boss's attack behavior
    /// </summary>
    public class BossAttackState : MonoBehaviour {

        /// <summary>
        /// The state pattern class
        /// </summary>
        static class States {
            public class State {

                /// <summary>
                /// To get access outside of this child class, boss is needed to access outside variables.
                /// </summary>
                protected BossAttackState bossAttack;

                /// <summary>
                /// Sets update up.
                /// </summary>
                /// <returns></returns>
                virtual public State Update() {

                    return null;
                }

                /// <summary>
                /// Referencing BossAttackState
                /// </summary>
                /// <param name="bossAttack"></param>
                virtual public void OnStart(BossAttackState bossAttack) {
                    this.bossAttack = bossAttack;
                }

                /// <summary>
                /// Tell when it is done
                /// </summary>
                virtual public void OnEnd() {

                }
            }

            //////////////////////////// Child Classes: 
            
            /// <summary>
            /// State when the boss has no targets to attack
            /// </summary>
            public class Idle : State {
                public override State Update() {

                    // Transitions:
                    if (bossAttack.healthAmt.health <= 0) // if the boss's health is at 0 or less, it will stop everything.
                        return null;

                    if (bossAttack.healthAmt.health <= bossAttack.minionSpawnPhasesFromHealth) { // if the boss health is less than or equal to health to trigger minion attack
                        bossAttack.minionSpawnPhasesFromHealth -= bossAttack.nextPhaseOfHealth; // sets next health phase
                        return new States.SpawnMinions(); // goes to SpawnMinion() state
                    }

                    // if the boss turret can see the player and has bullets in clip
                    if (bossAttack.bulletAmountInClip > 0 && bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance)) 
                        return new States.MiniGunAttack(); // goes to MiniGunAttack() state
                    
                    // if missiles are ready and boss can see player at a certain distance
                    if (bossAttack.misslesSpawned && bossAttack.misslesSpawned && bossAttack.CanSeeThing(bossAttack.player, bossAttack.missileDistance) && !bossAttack.CanSeeThing(bossAttack.player, bossAttack.missileDistance - 5)) {
                        return new States.HomingMissleAttack(); // goes to HomingMissileAttack() state
                    }


                    return null;
                }
                
            }

            /// <summary>
            /// State that spawns minions 
            /// </summary>
            public class SpawnMinions : State {

                public override State Update() {

                    // behaviour:
                    bossAttack.MinionSpawning(); // run MinionSpawning method to spawn the minions
                    SoundEffectBoard.EnemySpawnAlarmSound(); // plays sound

                    // transition:
                    return new States.Idle(); // goes to Idle() method
                }
            }

            /// <summary>
            /// State that fires the minigun
            /// </summary>
            public class MiniGunAttack : State {
                public override State Update() {
                    // behaviour
                    bossAttack.MachineGun(); // run the machine gun method
                    bossAttack.TurnTowardsTarget(); // runs method to turn towards the target

                    // transition
                    if (bossAttack.healthAmt.health <= 0) // if health is a 0 or less (When boss dies)
                        return new States.Idle(); // goes to Idle() state

                    if (bossAttack.healthAmt.health <= bossAttack.minionSpawnPhasesFromHealth) { // if the boss health is less than or equal to health to trigger minion attack
                        bossAttack.minionSpawnPhasesFromHealth -= bossAttack.nextPhaseOfHealth; // sets next health phase
                        return new States.SpawnMinions(); // goes to SpawnMinion() state
                    }

                    // if the boss turret can't see the enemy
                    if (!bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance)) return new States.Idle(); // goes to Idle() state

                    // if the boss has no bullets
                    if (bossAttack.bulletAmountInClip <= 0) return new States.Reload(bossAttack.reloadingTime); // goes to Reload() state

                    return null;
                }
            }

            /// <summary>
            /// State for boss to reload minigun
            /// </summary>
            public class Reload : State {

                float reloadTime = 5; // time to reload

                public Reload(float reload) {
                    reloadTime = reload; // re-sets reload time
                }

                public override State Update() {
                    // behaviour
                    reloadTime -= Time.deltaTime; // counts down the reload time


                    // transition

                    // if the boss health is less than or equal to health to trigger minion attack
                    if (bossAttack.healthAmt.health <= bossAttack.minionSpawnPhasesFromHealth) {
                        bossAttack.minionSpawnPhasesFromHealth -= bossAttack.nextPhaseOfHealth; // sets next health phase
                        return new States.SpawnMinions(); // goes to SpawnMinion() state
                    }

                    if (reloadTime <= 0) return new States.Idle(); // if reload time is at or below 0, goes to Idle() state
              
                    return null;
                }

                // At the end of the state
                public override void OnEnd() {
                    bossAttack.bulletAmountInClip = bossAttack.maxRoundsToHave; // refills bullets
                }
            }

            /// <summary>
            /// State for Homing Missiles to spawn
            /// </summary>
            public class HomingMissleAttack : State {

                public override State Update() {
                    // behavior:

                    // fires missiles
                    bossAttack.Missles(); 
                    bossAttack.Missles();

                    bossAttack.misslesSpawned = false; // turns missile spawned to false

                    return new States.Idle(); // goes to Idle() state
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
        public EnemyProjectiles prefabMachineGunBullets;

        /// <summary>
        /// amount of bullets the boss can fire
        /// </summary>
        private int bulletAmountInClip = 30;

        /// <summary>
        /// Max bullets it can set
        /// </summary>
        public int maxRoundsToHave = 30;

        /// <summary>
        /// Time it takes to reload
        /// </summary>
        public float reloadingTime = 0;

        /// <summary>
        /// Prefab of the missiles to fire
        /// </summary>
        public MissleScript prefabMissile;

        /// <summary>
        /// Left muzzle to fire bullets from
        /// </summary>
        public Transform leftMuzzle;

        /// <summary>
        /// Right muzzle to fire bullets from
        /// </summary>
        public Transform rightMuzzle;


        /// <summary>
        /// Bring in the missile to fire
        /// </summary>
        public MissleScript missile1;

        /// <summary>
        /// Bring in the missile to fire
        /// </summary>
        public MissleScript missile2;

        /// <summary>
        /// How far the missile can fire
        /// </summary>
        public float missileDistance = 25;

        /// <summary>
        /// Position for missile to spawn
        /// </summary>
        public Transform missilePos1;

        /// <summary>
        /// Position for missile to spawn
        /// </summary>
        public Transform missilePos2;


        /// <summary>
        /// Rounds per second to fire
        /// </summary>
        public float roundPerSec = 20;

        /// <summary>
        /// Amount of bullet to fire per second
        /// </summary>
        private float bulletAmountTime = 0;


        /// <summary>
        /// Time until missiles respawn
        /// </summary>
        public float missleRespawnTime = 5;

        /// <summary>
        /// if missiles have respawned or not
        /// </summary>
        bool misslesSpawned = true;

        /// <summary>
        /// Health of the boss
        /// </summary>
        public HealthScript healthAmt;

        /// <summary>
        /// When minions spawn at certain health of the boss
        /// </summary>
        private float minionSpawnPhasesFromHealth = 400;

        /// <summary>
        /// subtracts minionSpawnPhaseFromHealth to create next phase
        /// </summary>
        private float nextPhaseOfHealth = 100;


        /// <summary>
        /// To spawn minions 
        /// </summary>
        public EnemyBasicController minion;

        /// <summary>
        /// Point where minions would spawn
        /// </summary>
        public Transform minionSpawnPoint1;

        /// <summary>
        /// Point where minions would spawn
        /// </summary>
        public Transform minionSpawnPoint2;

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


        void Start() {
            // Getting components
            startingRotation = transform.localRotation; // gets the local rotation to rotate back to when there is no target
            healthAmt = GetComponentInParent<HealthScript>(); // gets HealthScript for health information
        }

        private void Update() {

            // Makes the script not run if boss health is 0 or below:
            if (healthAmt.health <= 0) {
                return;
            }

            // if nothing is assigned to the state, then make the state go to the Regular() state
            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update()); // makes the state run it's update method

            if (bulletAmountTime > 0) bulletAmountTime -= Time.deltaTime; // amount of bullets it would fire

            MissleSpawning(); // Spawning missiles

            // missile spawn it TRUE
            // FALSE when fired
            // Begins count down
            // When count down is at 0, spawns missle and ready to fire
        }

        /// <summary>
        /// Spawns new missiles to be fired when ready
        /// </summary>
        private void MissleSpawning() {
            if (missleRespawnTime > 0 && !misslesSpawned) { // if timer is greater than 0 and missilesSpawned is false
                missleRespawnTime -= Time.deltaTime;
            }

            if (missleRespawnTime <= 0) { // if timer is less than or equal to 0
                missile1 = Instantiate(prefabMissile, missilePos1.position, missilePos1.rotation); // spawns missile 1
                missile1.transform.parent = missilePos1; // make missilePos1 a parent of the missile prefab
                missile2 = Instantiate(prefabMissile, missilePos2.position, missilePos2.rotation); // spawns missile 2
                missile2.transform.parent = missilePos2; // make missilePos2 a parent of the missile prefab
                missleRespawnTime = 10; // time to respawn again
                misslesSpawned = true; // missiles spawned
                SoundEffectBoard.RocketMissileSound(); // plays the rocket/missile sound
            }
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
        /// Turns turret towards the target
        /// </summary>
        private void TurnTowardsTarget() {

            if (player) { // if player is set as a target
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
            } else {
                // figure out bone rotation, no target:

                transform.localRotation = AnimMath.Slide(transform.localRotation, startingRotation, .05f);
            }
        }

        /// <summary>
        /// Fires the machine gun to shoot at target
        /// </summary>
        void MachineGun() {
            if (bulletAmountTime > 0) return; // rate of fire manager

            EnemyProjectiles leftBullets = Instantiate(prefabMachineGunBullets, leftMuzzle.position, leftMuzzle.transform.rotation); // spawns bullet
            leftBullets.InitBullet(transform.forward * 30); // sets velocity of the projectile

            EnemyProjectiles RightBullets = Instantiate(prefabMachineGunBullets, rightMuzzle.position, rightMuzzle.transform.rotation);
            RightBullets.InitBullet(transform.forward * 30);

            SoundEffectBoard.BossShooting(); // plays the boss shooting sound

            bulletAmountInClip--; // removes a bullet
            bulletAmountTime = 1 / roundPerSec; // cause the rate of fire to happen
        }

        /// <summary>
        /// Cause the missiles to fire
        /// </summary>
        void Missles() {
            if (missile1) { // if missile1 is true
                missile1.timeToLaunch = true; // fires the missile
            }
            if (missile2) { // if missile 2 is true
                missile2.timeToLaunch = true; // fires the missile
            }

            SoundEffectBoard.RocketMissileSound(); // missile sound
        }

        /// <summary>
        /// Spawns the minions
        /// </summary>
        void MinionSpawning() {
            // for loop to spawn multiple minions
            for (int i = 0; i < 3; i++) {
                // spawns minions at point 1
                EnemyBasicController enemySpawnOnBoss1 = Instantiate(minion, minionSpawnPoint1.position + new Vector3(Random.Range(0f, 3f), 0, Random.Range(1.0f, 4.0f)), transform.rotation);
                // spawns minions at point 2
                EnemyBasicController enemySpawnOnBoss2 = Instantiate(minion, minionSpawnPoint2.position + new Vector3(Random.Range(0f, 3f), 0, Random.Range(1.0f, 4.0f)), transform.rotation);
            }
        }

        /// <summary>
        /// If the turret can see the target
        /// </summary>
        /// <param name="thing"></param>
        /// <param name="viewingDis"></param>
        /// <returns></returns>
        private bool CanSeeThing(Transform thing, float viewingDis) {

            if (!thing) return false; // uh... error

            Vector3 vToThing = thing.position - transform.position; // distance from boss to target

            // check distance
            if (vToThing.sqrMagnitude > viewingDis * viewingDis) { // can't see target
                return false; // Too far away to see...
            }

            // check direction
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // TODO: Check occulusion

            return true;
        }
    }
}