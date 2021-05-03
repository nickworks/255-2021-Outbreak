using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Velting
{
    public class EnemyBossController : MonoBehaviour
    {
        static class States
        {
            public class State
            {
                protected EnemyBossController enemy;

                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(EnemyBossController enemy)
                {
                    this.enemy = enemy;
                }
                virtual public void OnEnd()
                {

                }
            }

            //////////////////////// Children of State
            public class Idle : State 
            {
                public override State Update()
                {
                    //behavior:

                    //transition:
                    if (enemy.EnemyCanPursue() && enemy.health >700) return new States.Attack1();
                    if (enemy.EnemyCanPursue() && enemy.health > 400 && enemy.health < 700) return new States.Attack2();
                    if (enemy.EnemyCanPursue() && enemy.health < 400) return new States.Attack3();
                    if (enemy.health <= 0) return new States.Death();

                    return null;
                }

            }


            public class Attack1 : State 
            {

                
                public override State Update()
                {
                    //behavior:

                    
                    enemy.SpawnEnemy();
                      

                    //transitions:
                    if (!enemy.EnemyCanPursue()) return new States.Idle();
                    if (enemy.EnemyCanPursue() && enemy.health < 700) return new States.Attack2();

                    return null;
                }
            }


            public class Attack2 : State 
            {
                public override State Update()
                {
                    //behavior:
                    enemy.SpawnProjectile();

                    //transitions:
                    if (!enemy.EnemyCanPursue()) return new States.Idle();
                    if (enemy.EnemyCanPursue() && enemy.health < 400) return new States.Attack3();

                    return null;
                }
            }

            public class Attack3 : State 
            {
                public override State Update()
                {
                    //behavior:
                    enemy.SpawnMissile();

                    //transitions:
                    if (!enemy.EnemyCanPursue()) return new States.Idle();
                    if (enemy.health <= 0) return new States.Death();

                    return null;
                }
            }

            
            public class Death : State 
            {
                public override State Update()
                {
                    enemy.Death();
                    

                    return null;
                }


            }

        }

        private States.State state;

        private NavMeshAgent nav;

        

        public Transform enemySpawner;

        public Transform attackTarget;

        public EnemyBasicController minion;

        public EnemyBasicProjectile enemyBullet;

        public WinLossManager winner;

        public float enemyCooldown;

        public float maxEnemies = 5;

        public float currentEnemies;

        public float enemiesPerSecond = .25f;

        private Vector3 disToPlayer;
        private float pursuitDis = 50;
        public float health = 1020;

        public float bulletCooldown;
        public float roundsPerSecond = 5;

        public float missileCooldown;
        public float missilesPerSecond = 1;
        public EnemyHomingMissile missile;
        public Transform missileSpawner;

        public float nextLevelCountdown = 5;

        
        void Start()
        {
            nav = GetComponent<NavMeshAgent>();
        }
        void Update()
        {

            if (bulletCooldown > 0) bulletCooldown -= Time.deltaTime;

            if (enemyCooldown > 0) enemyCooldown -= Time.deltaTime;

            if (missileCooldown > 0) missileCooldown -= Time.deltaTime;
            
            if (state == null) SwitchState(new States.Idle());

            if (state != null) SwitchState(state.Update());
        }
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // Don't switch to nothing...

            // Call the current state's onEnd function.
            if (state != null) state.OnEnd();

            // Switch the state.
            state = newState;

            // Call the new state's onStart function.
            state.OnStart(this);
        }

        public void Death()
        {
            winner.isBossDead = true;
            Destroy(gameObject);
        }

        private bool EnemyCanPursue()
        {
            if (!attackTarget) return false;

            disToPlayer = attackTarget.transform.position - transform.position;

            if (disToPlayer.sqrMagnitude > pursuitDis) return false;
            if (disToPlayer.sqrMagnitude < pursuitDis) return true;

            return false;

        }

        private void OnTriggerEnter(Collider other)
        {
            Projectile bullet = other.GetComponent<Projectile>();

            health -= 10;
            bullet.BulletGone();
        }

        void SpawnEnemy()
        {
            if (enemyCooldown > 0) return;
            
                EnemyBasicController e = Instantiate(minion, enemySpawner.transform.position, Quaternion.identity);
                minion.attackTarget = attackTarget;
                minion.nav.SetDestination(minion.attackTarget.transform.position);

                currentEnemies++;
                enemyCooldown = 1 / enemiesPerSecond;
           

            


        }

        void SpawnProjectile()
        {
            if (bulletCooldown > 0) return; // Need to wait longer to shoot.
           


            EnemyBasicProjectile p = Instantiate(enemyBullet, missileSpawner.transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            
            bulletCooldown = 1 / roundsPerSecond;
        }

        void SpawnMissile()
        {
            if (missileCooldown > 0) return;

            EnemyHomingMissile m = Instantiate(missile, missileSpawner.transform.position, Quaternion.identity);
            missile.attackTarget = attackTarget;
            missile.nav.SetDestination(missile.attackTarget.transform.position);
            

            missileCooldown = 1 / missilesPerSecond;
        }
    }
}
