using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Howley
{
    public class BossStates : MonoBehaviour
    {
        /// <summary>
        /// Set up for the state design pattern
        /// </summary>
        static class States
        {
            public class State
            {
                // Reference the main bossstate script
                protected BossStates boss;

                /// <summary>
                /// Reference the override update
                /// </summary>
                /// <returns></returns>
                virtual public State Update()
                {
                    return null;
                }

                /// <summary>
                /// Reference the override onstart
                /// </summary>
                /// <param name="boss"></param>
                virtual public void OnStart(BossStates boss)
                {
                    this.boss = boss;
                }

                /// <summary>
                /// Reference the override onend
                /// </summary>
                virtual public void OnEnd()
                {

                }
            }

            ////////////////// Children of the State class

            public class Idle : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    boss.CanSeePlayer();
                    // TODO: Make Idle animation
                    // Transitions:
                    if (boss.canSeePlayer) boss.SwitchState(new States.Persuing());

                    if (boss.health.health <= 0) boss.SwitchState(new States.Death());
                    return null;
                }
            }
            public class Persuing : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    boss.MoveTheBoss();
                    boss.AttackCooldown();
                    boss.isAttacking = false;
                    // Transitions:
                    if (!boss.canSeePlayer) boss.SwitchState(new States.Idle());
                    if (boss.canSeePlayer && boss.vToPlayer.sqrMagnitude < boss.attackDis * boss.attackDis && boss.canAttack) boss.SwitchState(new States.Attack1());
                    if (boss.canSeePlayer && boss.vToPlayer.sqrMagnitude > boss.attackDis * boss.attackDis && boss.canAttack) boss.SwitchState(new States.Attack2());
                    if (boss.canSeePlayer && boss.vToPlayer.sqrMagnitude > boss.attackDis * boss.attackDis && boss.canAttack && boss.health.health < 300) boss.SwitchState(new States.Attack3());

                    if (boss.health.health <= 0) boss.SwitchState(new States.Death());
                    return null;
                }
            }
            public class ClimbWall : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    boss.ClimbWall();
                    // Transitions:

                    if (boss.health.health <= 0) boss.SwitchState(new States.Death());
                    return null;
                }
            }
            public class Stunned : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    boss.Stunned();
                    boss.isAttacking = false;
                    boss.DamageCooldown();

                    // Transitions:
                    if (boss.damageCooldown <= 0) boss.SwitchState(new States.Idle());
                    if (boss.health.health <= 0) boss.SwitchState(new States.Death());
                    return null;
                }
            }
            public class Death : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:

                    // Transitions:


                    return null;
                }
            }
            public class Attack1 : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    boss.DoAttack1();
                    boss.isAttacking = true;
                    boss.attackCooldown = 0;
                    boss.canAttack = false;
                    // Transitions:
                    if (boss.attackTimer >= 1) 
                    {
                        boss.attackTimer = 0;
                        boss.SwitchState(new States.Idle());
                    }

                    if (boss.health.health <= 0) boss.SwitchState(new States.Death());

                    return null;
                }
            }
            public class Attack2 : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    boss.DoAttack2();
                    boss.isAttacking = true;
                    boss.attackCooldown = 0;
                    boss.canAttack = false;
                    // Transitions:
                    if (boss.attack2Timer >= 1)
                    {
                        boss.attack2Timer = 0;
                        boss.SwitchState(new States.Idle());
                    }

                    if (boss.health.health <= 0) boss.SwitchState(new States.Death());
                    return null;
                }
            }
            public class Attack3 : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    boss.DoAttack3();
                    boss.isAttacking = true;
                    boss.attackCooldown = 0;
                    boss.canAttack = false;
                    // Transitions:
                    if (boss.attack3Timer >= 1)
                    {
                        boss.attack3Timer = 0;
                        boss.SwitchState(new States.Idle());
                    }
                    
                    if (boss.health.health <= 0) boss.SwitchState(new States.Death());
                    return null;
                }
            }
            public class Attack4 : State
            {
                /// <summary>
                /// Call this state's update every game tick
                /// </summary>
                /// <returns></returns>
                public override State Update()
                {
                    // Behavior:
                    // Transitions:
                    return null;
                }
            }
        }

        // Hold reference to the list of states
        private States.State state;

        // Set pawn to CharacterController
        private CharacterController pawn;

        /// <summary>
        /// Reference the healthsystem on this object
        /// </summary>
        public HealthSystem health;

        // Reference the target to move towards
        public Transform attackTarget;

        /// <summary>
        /// Reference the Boss's neck bone in the inspector
        /// </summary>
        public Transform neckBone;

        /// <summary>
        /// Reference the Boss's torso bone in the inspector
        /// </summary>
        public Transform torsoBone;

        /// <summary>
        /// Reference the boss's left shoulder in the inspector
        /// </summary>
        public Transform shoulderLeft;

        /// <summary>
        /// Reference the Boss's Right shoulder in the inspector
        /// </summary>
        public Transform shoulderRight;

        /// <summary>
        /// Reference the Boss's left hip in the inspector
        /// </summary>
        public Transform hipLeft;

        /// <summary>
        /// Reference the Boss's right hip in the inspector
        /// </summary>
        public Transform hipRight;

        /// <summary>
        /// Reference the boss's tail bone in the inspector
        /// </summary>
        public Transform tailBone;

        /// <summary>
        /// Hold reference to the projectile to spawn on attack
        /// </summary>
        public Projectile prefabProjectile;

        /// <summary>
        /// This variable holds the direction towards the player
        /// </summary>
        private Vector3 vToPlayer;

        /// <summary>
        /// This variable is multiplied to direction to give speed to the boss
        /// </summary>
        private float moveSpeed = .5f;

        /// <summary>
        /// How far can the boss see
        /// </summary>
        public float visDis = 10;

        /// <summary>
        /// How far away the player has to be from the boss for it to attack.
        /// </summary>
        public float attackDis = 6;

        /// <summary>
        /// The ange at which the boss can see 
        /// </summary>
        public float visCone = 160;

        /// <summary>
        /// This variable counts the amount of time in the attack state
        /// </summary>
        private float attackTimer = 0;

        /// <summary>
        /// This variable counts the amount of time in the attack2 state
        /// </summary>
        private float attack2Timer = 0;

        /// <summary>
        /// This variable counts the amount of time in the attack3 state
        /// </summary>
        private float attack3Timer = 0;

        /// <summary>
        /// This variable sets the timer for the boss not being able to attack
        /// </summary>
        private float attackCooldown = 0;

        /// <summary>
        /// This variable sets the timer for the boss not being able to take damage
        /// </summary>
        private float damageCooldown = 0;

        /// <summary>
        /// Depending on the vision distance, and cone.
        /// </summary>
        private bool canSeePlayer = false;

        /// <summary>
        ///  This variable is true or false depending on if the boss is able to attack
        /// </summary>
        private bool canAttack = false;

        /// <summary>
        /// This variable is true or false depending on if the boss can take damage
        /// </summary>
        private bool canTakeDamage = false;

        /// <summary>
        /// This variable is true or false depending on if the boss is attacking
        /// </summary>
        private bool isAttacking = false;


        void Start()
        {
            // Set pawn to get the CharacterController Component
            pawn = GetComponent<CharacterController>();

            health = GetComponent<HealthSystem>();
        }

        /// <summary>
        /// Update is called every game tick
        /// </summary>
        void Update()
        {
            // If we are not in a state, use the Idle state
            if (state == null) SwitchState(new States.Idle());

            // If we are in a state, look for the switch condition in the current state's update
            if (state != null) SwitchState(state.Update());

            // Pass the result of the canseeplayer function into a boolean.
            canSeePlayer = CanSeePlayer();
        }

        /// <summary>
        /// This funtion switches between the boss's states
        /// </summary>
        /// <param name="newState"></param>
        void SwitchState(States.State newState)
        {
            if (newState == null) return; // Can't switch to nothing

            // Call the previous state's on end 
            if (state != null) state.OnEnd();

            // Switch to the new state
            state = newState;

            // Call the new state's on start function
            state.OnStart(this);
        }

        /// <summary>
        /// This function returns true or false based on the vector to the player, and the cone of vision for the boss.
        /// </summary>
        /// <returns></returns>
        private bool CanSeePlayer()
        {
            if (!attackTarget) return false;

            vToPlayer = attackTarget.transform.position - transform.position;

            if (vToPlayer.sqrMagnitude > visDis * visDis) return false;
            if (Vector3.Angle(transform.forward, vToPlayer) > visCone) return false;

            if (vToPlayer.sqrMagnitude < visDis * visDis && Vector3.Angle(transform.forward, vToPlayer) < visCone) return true;

            return false;
        }

        /// <summary>
        /// This function is a cooldown called after the boss does an attack
        /// </summary>
        void AttackCooldown()
        {
            attackCooldown += Time.deltaTime;

            if (attackCooldown >= 5) canAttack = true;
        }

        /// <summary>
        /// This function is called for the boss to move towards the player
        /// </summary>
        void MoveTheBoss()
        {
            if (!attackTarget) return;

            // Turn the boss to look at the player
            Quaternion rotateToPlayer = Quaternion.LookRotation(vToPlayer, Vector3.up);

            // Get local Euler angles before rotation
            Vector3 euler1 = transform.localEulerAngles;
            Quaternion previousRot = transform.rotation;

            // Do rotation
            transform.rotation = rotateToPlayer;

            // Get new local euler angles 
            float eulerY = transform.localEulerAngles.y;
            float eulerZ = transform.localEulerAngles.z;

            // Revert the rotation
            transform.rotation = previousRot;

            // Ease the rotation to the target.
            transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(0, eulerY, eulerZ), .3f);

            // Move in the direction of the player
            pawn.SimpleMove(vToPlayer * moveSpeed);
        }

        /// <summary>
        /// This function is called when the boss is able to do it's first attack
        /// </summary>
        void DoAttack1()
        {
            Quaternion startingLeftArmRot = shoulderLeft.transform.localRotation;
            Quaternion targetLeftArmRot = startingLeftArmRot * Quaternion.Euler(50, 0, 10);

            attackTimer += Time.deltaTime;

            if (attackTimer <= .5f) shoulderLeft.transform.localRotation = AnimMath.Slide(startingLeftArmRot, targetLeftArmRot, .01f);
            if (attackTimer >= .51f) shoulderLeft.transform.localRotation = AnimMath.Slide(shoulderLeft.transform.localRotation, Quaternion.identity, .01f);
        }

        /// <summary>
        /// This function is called when the boss is able to do it's second attack
        /// </summary>
        void DoAttack2()
        {
            Quaternion startingRot = transform.localRotation;
            Quaternion startingTailRot = tailBone.transform.localRotation;

            Quaternion targetTorsoRot = startingRot * Quaternion.Euler(0, 180, 0);
            Quaternion targetTailRot = startingTailRot * Quaternion.Euler(0, 45, 0);

            attack2Timer += Time.deltaTime;

            if (attack2Timer >= .25f) targetTailRot = startingTailRot * Quaternion.Euler(0, -90, 0);
            if (attack2Timer > .5f) targetTorsoRot = Quaternion.identity;

            transform.localRotation = AnimMath.Slide(startingRot, targetTorsoRot, .001f);
            tailBone.transform.localRotation = AnimMath.Slide(startingTailRot, targetTailRot, .001f);
        }

        /// <summary>
        /// This function is called when the boss is able to do it's third attack
        /// </summary>
        void DoAttack3()
        {
            Projectile projectile;
            Quaternion startingNeckRot = neckBone.transform.localRotation;

            attack3Timer += Time.deltaTime;

            if (attack3Timer >= .75f)
            {
               projectile = Instantiate(prefabProjectile, neckBone.transform.position, neckBone.rotation);
            }
            Quaternion targetNeckRot = startingNeckRot * Quaternion.Euler(60, 0, 0);

            transform.localRotation = AnimMath.Slide(startingNeckRot, targetNeckRot, .005f);
        }       

        /// <summary>
        /// This function is called when the boss takes damage from the player
        /// </summary>
        void Stunned()
        {
            Quaternion startingNeckRot = neckBone.transform.localRotation;
            Quaternion startingLeftArmRot = shoulderLeft.transform.localRotation;
            Quaternion startingRightArmRot = shoulderRight.transform.localRotation;

            Quaternion targetNeckRot = startingNeckRot * Quaternion.Euler(50, 0, 0);
            Quaternion targetLeftArmRot = startingLeftArmRot * Quaternion.Euler(0, 0, 40);
            Quaternion targetRightArmRot = startingRightArmRot * Quaternion.Euler(0, 0, 40);

            neckBone.transform.localRotation = AnimMath.Slide(startingNeckRot, targetNeckRot, .003f);
            shoulderLeft.transform.localRotation = AnimMath.Slide(startingLeftArmRot, targetLeftArmRot, .001f);
            shoulderRight.transform.localRotation = AnimMath.Slide(startingRightArmRot, targetRightArmRot, .001f);

        }
        void ClimbWall()
        {

        }

        /// <summary>
        /// this function is called when the projectile enters the boss's collision box
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();

            if (isAttacking)
            {
                if (pm)
                {
                    HealthSystem playerHealth = pm.GetComponent<HealthSystem>();
                    if (playerHealth && canTakeDamage)
                    {
                        playerHealth.Damage(50);
                    }
                }
            }

            if (!isAttacking && canTakeDamage)
            {
                SwitchState(new States.Stunned());
                health.Damage(25);
            }
            
        }

        /// <summary>
        /// This function is called after the boss takes damage
        /// </summary>
        void DamageCooldown()
        {
            if (damageCooldown >= 0) canTakeDamage = false;

            damageCooldown -= Time.deltaTime;

            if (damageCooldown <= 0) canTakeDamage = true;
        }
    }
}

