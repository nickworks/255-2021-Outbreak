using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Szczesniak {
    public class BossAttackState : MonoBehaviour {

        static class States {
            public class State {

                protected BossAttackState bossAttack;

                virtual public State Update() {

                    return null;
                }

                virtual public void OnStart(BossAttackState bossAttack) {
                    this.bossAttack = bossAttack;
                }

                virtual public void OnEnd() {

                }
            }

            //////////////////////////// Child Classes: 
            ///

            public class Idle : State {
                public override State Update() {
                    // Behaviour:

                    // Transitions:
                    if (bossAttack.healthAmt.health <= 0)
                        return null;

                    if (bossAttack.healthAmt.health <= bossAttack.minionSpawnPhasesFromHealth) {
                        bossAttack.minionSpawnPhasesFromHealth -= bossAttack.nextPhaseOfHealth;
                        return new States.SpawnMinions();
                    }

                    if (bossAttack.bulletAmountInClip > 0 && bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance)) 
                        return new States.MiniGunAttack();

                    if (bossAttack.misslesSpawned && bossAttack.misslesSpawned && bossAttack.CanSeeThing(bossAttack.player, bossAttack.missileDistance) && !bossAttack.CanSeeThing(bossAttack.player, bossAttack.missileDistance - 5)) {
                        return new States.HomingMissleAttack();
                    }


                    return null;
                }
                
            }

            public class SpawnMinions : State {

                public override State Update() {
                    bossAttack.MinionSpawning();

                    return new States.Idle();
                }
            }

            public class MiniGunAttack : State {
                public override State Update() {
                    // behaviour
                    bossAttack.MachineGun();
                    bossAttack.TurnTowardsTarget();

                    // transition
                    if (bossAttack.healthAmt.health <= 0)
                        return new States.Idle();

                    if (bossAttack.healthAmt.health <= bossAttack.minionSpawnPhasesFromHealth) {
                        bossAttack.minionSpawnPhasesFromHealth -= bossAttack.nextPhaseOfHealth;
                        return new States.SpawnMinions();
                    }

                    if (!bossAttack.CanSeeThing(bossAttack.player, bossAttack.viewingDistance)) return new States.Idle();

                    if (bossAttack.bulletAmountInClip <= 0) return new States.Reload(bossAttack.reloadingTime);

                    return null;
                }
            }

            public class Reload : State {

                float reloadTime = 5;

                public Reload(float reload) {
                    reloadTime = reload;
                }

                public override State Update() {
                    // behaviour
                    reloadTime -= Time.deltaTime;


                    // transition

                    if (bossAttack.healthAmt.health <= bossAttack.minionSpawnPhasesFromHealth) {
                        bossAttack.minionSpawnPhasesFromHealth -= bossAttack.nextPhaseOfHealth;
                        return new States.SpawnMinions();
                    }

                    if (reloadTime <= 0) return new States.Idle();
              
                    return null;
                }

                public override void OnEnd() {
                    bossAttack.bulletAmountInClip = bossAttack.maxRoundsToHave;
                }
            }

            public class HomingMissleAttack : State {

                public override State Update() {
                    // behavior:
                    bossAttack.Missles();
                    bossAttack.Missles();

                    bossAttack.misslesSpawned = false;

                    return new States.Idle();
                }
            }
        }

        private States.State state;

        public Projectile prefabMachineGunBullets;
        private int bulletAmountInClip = 30;
        public int maxRoundsToHave = 30;
        public float reloadingTime = 0;

        public MissleScript prefabMissile;

        public Transform leftMuzzle;
        public Transform rightMuzzle;

        public MissleScript missile1;
        public MissleScript missile2;
        public float missileDistance = 25;
        public Transform missilePos1;
        public Transform missilePos2;


        public float roundPerSec = 20;
        private float bulletAmountTime = 0;

        public float missleRespawnTime = 5;
        bool misslesSpawned = true;
        public HealthScript healthAmt;
        private float minionSpawnPhasesFromHealth = 150;
        private float nextPhaseOfHealth = 50;

        public EnemyBasicController minion; 

        public float viewingAngle = 90;
        public float viewingDistance = 20;

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
            startingRotation = transform.localRotation;
            healthAmt = GetComponentInParent<HealthScript>();
        }

        private void Update() {

            // Makes the script not run if boss health is 0 or below:
            if (healthAmt.health <= 0)
                return;

            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update());

            if (bulletAmountTime > 0) bulletAmountTime -= Time.deltaTime;

            print(misslesSpawned);

            MissleSpawning();

            // missile spawn it TRUE
            // FALSE when fired
            // Begins count down
            // When count down is at 0, spawns missle and ready to fire
        }

        private void MissleSpawning() {
            if (missleRespawnTime > 0 && !misslesSpawned) {
                missleRespawnTime -= Time.deltaTime;
            }

            if (missleRespawnTime <= 0) {
                missile1 = Instantiate(prefabMissile, missilePos1.position, missilePos1.rotation);
                missile1.transform.parent = missilePos1;
                missile2 = Instantiate(prefabMissile, missilePos2.position, missilePos2.rotation);
                missile2.transform.parent = missilePos2;
                missleRespawnTime = 10;
                misslesSpawned = true;
            }
        }

        void SwitchState(States.State newState) {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);
        }

        private void TurnTowardsTarget() {

            if (player) {
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

        void MachineGun() {
            if (bulletAmountTime > 0) return;

            Projectile leftBullets = Instantiate(prefabMachineGunBullets, leftMuzzle.position, Quaternion.identity);
            leftBullets.InitBullet(transform.forward * 30);

            Projectile RightBullets = Instantiate(prefabMachineGunBullets, rightMuzzle.position, Quaternion.identity);
            RightBullets.InitBullet(transform.forward * 30);

            bulletAmountInClip--;
            bulletAmountTime = 1 / roundPerSec;
        }

        void Missles() {
            if (missile1) {
                missile1.timeToLaunch = true;
            }
            if (missile2) {
                missile2.timeToLaunch = true;
            }
        }

        void MinionSpawning() {
            for (int i = 0; i < 5; i++) {
                EnemyBasicController enemySpawnOnBoss = Instantiate(minion, transform.position - new Vector3(Random.Range(1.0f, 4.0f), 0, Random.Range(1.0f, 4.0f)), transform.rotation);
            }
        }

        private bool CanSeeThing(Transform thing, float viewingDis) {

            if (!thing) return false; // uh... error

            Vector3 vToThing = thing.position - transform.position;

            // check distance
            if (vToThing.sqrMagnitude > viewingDis * viewingDis) {
                return false; // Too far away to see...
            }

            // check direction
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            // TODO: Check occulusion

            return true;
        }
    }
}