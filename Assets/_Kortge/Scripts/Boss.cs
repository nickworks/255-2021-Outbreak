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
            Charge,
            Attack,
            Thrust,
            Cooldown,
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
                float particleTime;
                public override State Update()
                {
                    particleTime -= Time.deltaTime;
                    if (particleTime <= 0) return new States.Charge();
                    return null;
                }
                public override void OnStart(Boss boss)
                {
                    boss.SmokeOut();
                    particleTime = boss.reactionTime;
                    base.OnStart(boss);
                }
                public override void OnEnd()
                {
                    boss.SmokeIn();
                    boss.focused = true;
                    boss.colliding = false;
                }
            }
            public class Charge : State
            {
                float chargeTime;
                public override State Update()
                {
                    chargeTime -= Time.deltaTime;
                    if(chargeTime <= 0.25f ) boss.animator.SetBool("BeamReady", true);
                    if (chargeTime <= 0) return new States.Attack();
                    return null;
                }
                public override void OnStart(Boss boss)
                {
                    chargeTime = boss.reactionTime;
                    base.OnStart(boss);
                }

                public override void OnEnd()
                {
                    boss.animator.SetBool("BeamReady", false);
                }
            }

            public class Attack : State
            {
                float beamTime;
                float shotDelay;
                float shotTime;
                public override State Update()
                {
                    beamTime -= Time.deltaTime;
                    if (beamTime <= 0) return new States.Thrust();
                    if (shotTime <= 0)
                    {
                        boss.Beam((boss.burst / 2) + 5);
                        shotTime = shotDelay;
                    }
                    else shotTime -= Time.deltaTime;
                    return null;
                }

                public override void OnStart(Boss boss)
                {
                    beamTime = boss.reactionTime;
                    shotDelay = boss.reactionTime/boss.burst;
                    shotTime = shotDelay;
                    base.OnStart(boss);
                }
            }

            public class Thrust : State
            {
                public override State Update()
                {
                    boss.controller.SimpleMove(boss.transform.forward * ((boss.burst/2)+5));
                    if (boss.colliding) return new States.Cooldown();
                    return null;
                }
                public override void OnStart(Boss boss)
                {
                    boss.focused = false;
                    base.OnStart(boss);
                }
            }

            public class Cooldown : State
            {
                float cooldownTime;
                public override State Update()
                {
                    cooldownTime -= Time.deltaTime;
                    if (boss.hit) return new States.Hit();
                    if (cooldownTime <= 0) return new States.Teleport();
                    return null;
                }
                public override void OnStart(Boss boss)
                {
                    cooldownTime = boss.reactionTime * 2;
                    base.OnStart(boss);
                }
                public override void OnEnd()
                {
                    boss.animator.SetTrigger("Beam Finish");

                }
            }

            public class Hit : State
            {
                float hitTime = 1f;
                bool transparent = false;
                public override State Update()
                {
                    hitTime -= Time.deltaTime;
                    if (transparent)
                    {
                        boss.sprite.color = new Color(0.2f, 0.2f, 0.2f, 1);
                        transparent = false;
                    }
                    else
                    {
                        boss.sprite.color = Color.clear;
                        transparent = true;
                    }
                    if (hitTime <= 0) {
                        boss.sprite.color = new Color(0.2f, 0.2f, 0.2f, 1);
                        return new States.Teleport();
                    }
                    return null;
                }

                public override void OnStart(Boss boss)
                {
                    boss.animator.SetTrigger("Beam Finish");
                    boss.hit = false;
                    //boss.rigidBody.AddForce(boss.transform.position - boss.player.position, ForceMode2D.Impulse);
                    //boss.collider2d.enabled = false;
                    base.OnStart(boss);
                }

                public override void OnEnd()
                {
                    //boss.controller.enabled = true;
                }
            }

            public class Death : State
            {
            }
        }

        private States.State state;
        public ParticleSystem smokeOut;
        public ParticleSystem smokeIn;
        public Transform player;
        public Projectile beamPrefab;
        private float reactionTime;
        private Health health;
        private int burst;
        private SpriteRenderer sprite;
        public bool hit = false;
        private Animator animator;
        private ParticleSystem gleam;
        public GameObject laser;
        private bool colliding;
        private bool focused = true;
        private CharacterController controller;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponentInChildren<Animator>();
            gleam = GetComponentInChildren<ParticleSystem>();
            controller = GetComponent<CharacterController>();
            reactionTime = 0.2f + (0.02f * health.health);
            burst = 10 - health.health + 1;
        }

        // Update is called once per frame
        void Update()
        {
            if (state == null) SwitchState(new States.Teleport());

            if (state != null)
            {
                if (state != null) SwitchState(state.Update());
            };

            if(focused)LookAtPlayer();

            //if (timerSpawnBullt <= 0)

            reactionTime = 0.2f + (0.02f * health.health);
            burst = 10 - health.health + 1;
            print(state);
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
            Instantiate(smokeOut, transform.position, new Quaternion(0,0,0,0));
            transform.position = Vector3.up * (20);
        }

        void SmokeIn()
        {
            transform.position = (Vector3.right * Random.Range(-9f, 9f)) + (Vector3.forward * Random.Range(-4f, 4f));
            Instantiate(smokeIn, transform.position, new Quaternion(0, 0, 0, 0));
        }

        void LookAtPlayer()
        {
            Vector3 vectorToHitPos = player.position - transform.position;

            float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

            angle /= Mathf.PI;
            angle *= 180; // Converts from radians to half-circles to degrees.

            transform.eulerAngles = new Vector3(0, angle, 0);
        }

        void Beam(float speed)
        {
            Projectile beam = Instantiate(beamPrefab, transform.position + transform.forward, transform.rotation);
            beam.InitBullet(transform.forward * speed);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            GameObject wall = hit.gameObject;
            if(wall.CompareTag("Wall")) colliding = true;
        }
    }
}