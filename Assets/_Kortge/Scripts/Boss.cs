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
                float readyTime = 1;
                public override State Update()
                {
                    readyTime -= Time.deltaTime;
                    if (readyTime <= 0) return new States.Teleport();
                    return null;
                }
                public override void OnStart(Boss boss)
                {
                    boss.SmokeIn();
                    base.OnStart(boss);
                }
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
                float beamTime = 1;
                float shotDelay;
                float shotTime;
                public override State Update()
                {
                    beamTime -= Time.deltaTime;
                    shotTime -= Time.deltaTime;
                    if (beamTime <= 0) return new States.Thrust();
                    if (shotTime <= 0)
                    {
                        boss.Beam(5 + (6-boss.health.health));
                        shotTime = shotDelay;
                    }
                    else shotTime -= Time.deltaTime;
                    return null;
                }

                public override void OnStart(Boss boss)
                {
                    shotDelay = 1f/(12f*(6-boss.health.health));
                    shotTime = shotDelay;
                    base.OnStart(boss);
                }
            }

            public class Thrust : State
            {
                List<AfterImage> images = new List<AfterImage>();
                public override State Update()
                {
                    boss.controller.SimpleMove(boss.transform.forward * ((6-boss.health.health) * 4));
                    AfterImage();
                    if (boss.colliding) return new States.Cooldown();
                    return null;
                }

                private void AfterImage()
                {
                    AfterImage newImage = Instantiate(boss.afterImage, boss.transform.position, boss.transform.rotation);
                    newImage.destroyTime = 1f / boss.health.health;
                    images.Add(newImage);
                }

                public override void OnStart(Boss boss)
                {
                    boss.maidenCue = true;
                    boss.focused = false;
                    base.OnStart(boss);
                }

                public override void OnEnd()
                {
                    foreach (AfterImage image in images)
                    {
                        if(image != null)Destroy(image.gameObject);
                    }
                }
            }

            public class Cooldown : State
            {
                float cooldownTime;
                float distance = 3;
                public override State Update()
                {
                    cooldownTime -= Time.deltaTime;
                    if (boss.hit)
                    {
                        if (boss.health.health <= 0) return new States.Death();
                        boss.hit = false;
                        return new States.Hit();
                    }
                    if (boss.earlyTeleport)
                    {
                        if(boss.player != null)distance = Vector3.Distance(boss.player.position, boss.transform.position);
                    }
                    if (cooldownTime <= 0 || distance < 2) return new States.Teleport();
                    return null;
                }
                public override void OnStart(Boss boss)
                {
                    boss.sweat.Play();
                    boss.health.vulnerable = true;
                    cooldownTime = boss.reactionTime;
                    base.OnStart(boss);
                }
                public override void OnEnd()
                {
                    boss.sweat.Stop();
                    boss.health.vulnerable = false;
                    boss.animator.SetTrigger("Beam Finish");
                }
            }

            public class Hit : State
            {
                float hitTime = 0.2f;
                public override State Update()
                {
                    hitTime -= Time.deltaTime;
                    if (hitTime <= 0) return new States.Teleport();
                    return null;
                }

                public override void OnStart(Boss boss)
                {
                    boss.animator.SetTrigger("Beam Finish");
                    hitTime = boss.reactionTime;
                    //boss.rigidBody.AddForce(boss.transform.position - boss.player.position, ForceMode2D.Impulse);
                    //boss.collider2d.enabled = false;
                }

                public override void OnEnd()
                {
                    //boss.controller.enabled = true;
                }
            }

            public class Death : State
            {
                public override void OnStart(Boss boss)
                {
                    boss.dead = true;
                }
            }
        }

        private States.State state;
        public ParticleSystem smokeOut;
        public ParticleSystem smokeIn;
        public Transform player;
        public Projectile beamPrefab;
        public float reactionTime;
        private Health health;
        public bool hit = false;
        private Animator animator;
        public GameObject laser;
        private bool colliding;
        private bool focused = true;
        private CharacterController controller;
        public AfterImage afterImage;
        private bool earlyTeleport = false;
        public bool maidenCue = false;
        public bool dead;
        private ParticleSystem sweat;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            animator = GetComponentInChildren<Animator>();
            controller = GetComponent<CharacterController>();
            reactionTime = 1f;
            sweat = GetComponentInChildren<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (state == null) SwitchState(new States.Draw());

            if (state != null)
            {
                if (state != null) SwitchState(state.Update());
            };

            if(focused)LookAtPlayer();

            reactionTime = health.health * 0.2f;

            if (health.health <= 3) earlyTeleport = true;

            //if (timerSpawnBullt <= 0)
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
            transform.position = (Vector3.right * Random.Range(-9f, 9f)) + (Vector3.forward * Random.Range(-4f, 4f)) + (Vector3.up/2);
            Instantiate(smokeIn, transform.position, new Quaternion(0, 0, 0, 0));
        }

        void LookAtPlayer()
        {
            if (player != null)
            {
                Vector3 vectorToHitPos = player.position - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI;
                angle *= 180; // Converts from radians to half-circles to degrees.

                transform.eulerAngles = new Vector3(0, angle, 0);
            }
            else return;
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