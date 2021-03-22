using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class Boss : MonoBehaviour
    {
        public enum BossState
        {
            Draw,
            Teleport,
            Beam,
            Thrust,
            Spin,
            Hit,
            Death
        }

        public static class States
        {
            public class State
            {
                protected Boss boss;
                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(Boss boss)
                {
                    this.boss = boss;
                }
                virtual public void OnEnd()
                {

                }
            }

            public class Draw : State
            {
            }

            public class Teleport : State
            {
                float particleTime = 0.5f;
                public override void OnStart(Boss boss)
                {
                    boss.SmokeOut();
                }
                public override State Update()
                {
                    particleTime -= Time.deltaTime;
                    if (particleTime <= 0) return new States.Beam();
                    return null;
                }
            }

            public class Beam : State
            {
                float beamTime;
                float beamStartTime;
                float beamEndTime;
                float shotDelay;
                float shotTime;
                public override State Update()
                {
                    print(beamStartTime);
                    beamTime -= Time.deltaTime;
                    if (beamTime <= beamStartTime && beamTime >= beamEndTime)
                    {
                        if (shotTime <= 0)
                        {
                            boss.Beam(9f);
                            shotTime = shotDelay;
                        }
                        else shotTime -= Time.deltaTime;
                    }
                    else if (beamTime <= 0) return new States.Teleport();
                    return null;
                }

                public override void OnStart(Boss boss)
                {
                    beamTime = boss.reactionTime * 3;
                    beamStartTime = boss.reactionTime * 2;
                    beamEndTime = boss.reactionTime;
                    boss.SmokeIn();
                    shotDelay = boss.reactionTime/boss.burst;
                    shotTime = shotDelay;
                    base.OnStart(boss);
                }
            }

            public class Thrust : State
            {
            }

            public class Spin : State
            {
            }

            public class Hit : State
            {

            }

            public class Death : State
            {
            }
        }

        private States.State state;
        public ParticleSystem smoke;
        public Transform player;
        public Projectile beamPrefab;
        private float reactionTime = 2f;
        private Health health;
        private int burst = 7;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (state == null) SwitchState(new States.Teleport());

            if (state != null)
            {
                if (state != null) SwitchState(state.Update());
            };

            LookAtPlayer();

            //if (timerSpawnBullt <= 0)

            if (health.health == 6) {
                reactionTime = 1f;
                burst = 14;
            }
            if (health.health == 3) { reactionTime = 0.5f;
                burst = 28;
            }

        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;

            if (state != null) state.OnEnd();

            state = newState;

            state.OnStart(this);
        }

        void SmokeOut()
        {
            Instantiate(smoke, transform.position, transform.rotation);
            transform.position = transform.up * (-20);
        }

        void SmokeIn()
        {
            transform.position = (Vector3.right * Random.Range(-10f, 10f)) + (Vector3.up * Random.Range(-5f, 5f));
            Instantiate(smoke, transform.position, transform.rotation);
        }

        void LookAtPlayer()
        {
            Vector3 vectorToHitPos = player.position - transform.position;

            float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.y);

            angle /= Mathf.PI;
            angle *= 180; // Converts from radians to half-circles to degrees.

            transform.eulerAngles = new Vector3(0, 0, -angle);
        }

        void Beam(float speed)
        {
            Projectile beam = Instantiate(beamPrefab, transform.position + transform.up, Quaternion.identity);
            beam.InitBullet(transform.up * speed);
        }
    }
}