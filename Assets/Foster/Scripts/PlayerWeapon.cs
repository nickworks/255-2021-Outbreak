using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foster
{
    public class PlayerWeapon : MonoBehaviour
    {
        public PlayerMovement PM;
        public Projectile prefabProjectile;
        public Effects HealEffect;

        static class States
        {
            public class State 
            {
                protected PlayerWeapon weapon;
                virtual public State Update()
                {
                    return null;
                }

                virtual public void OnStart(PlayerWeapon weapon)
                {
                    this.weapon = weapon;
                }
                virtual public void OnEnd()
                {

                }
            }
            public class Regular : State 
            {
                public override State Update()
                {
                    //behaviour

                    //transitions
                    if (Input.GetButtonDown("Fire1"))
                    {
                        

                        return new States.Attacking();

                    }
                    
                    if (Input.GetButtonDown("Fire2"))
                    {
                       

                        return new States.Healing();

                    }
                    return null;
                }
            }
            public class Attacking : State 
            {
                public override State Update()
                {
                    //Behaviour
                    weapon.SpawnProjectile();


                    //transition
                    if (!Input.GetButtonDown("Fire1")) return new States.Regular();

                    return null;
                }
            }
           
            public class Healing: State
            {
                public override State Update()
                {

                    weapon.SpawnHealthEffect();
                 

                    //transition
                    if (!Input.GetButtonDown("Fire2")) return new States.Regular();

                    return null;
                }
            }
        }

        private States.State state;

        public int maxRoundsInClips = 8;
        private int roundsInClip = 8;
        public float reloadTime = 1;
        /// <summary>
        /// How many bullets to spawn per second. We use this calculate the timing between bullets.
        /// </summary>
        public float roundsPerSecond = 5;
        /// <summary>
        /// how many seonds till we can fire again
        /// </summary>
        private float timerSpawnBullet = 0;

        private float healingSpellCooldown = 0;
        private float spellCooldown = 0;

        void Start()
        {

        }


        void Update()
        {
           if(timerSpawnBullet >0) timerSpawnBullet -= Time.deltaTime;
            if (state == null) SwitchState(new States.Regular());

            //call stat.update()
            //switch to the return state
            if (state != null) SwitchState(state.Update());
            if (healingSpellCooldown >= 0) healingSpellCooldown -= Time.deltaTime;
            if (spellCooldown >= 0) spellCooldown -= Time.deltaTime;


            //print(healingSpellCooldown);

        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;
            if (state != null) state.OnEnd();

            state = newState;
            state.OnStart(this);
        }

        void SpawnProjectile()
        {
           // if (spellCooldown <= 0)
            //{
                if (PlayerMovement.mana >= 10)
                {

                    Projectile p = Instantiate(prefabProjectile, transform.position, Quaternion.identity);
                    p.InitBullet(transform.forward * 20);

                    PlayerMovement.mana -= 10;
                    PM.manaRegenTimer = 1f;
                    spellCooldown = 1.5f;
                }
          ////  }
            //if (spellCooldown >= .01) return;
            if (PlayerMovement.mana <= 0) return;

        }
        void SpawnHealthEffect()
        {
            if(healingSpellCooldown <= 0)
            {
                healingSpellCooldown = 5f;
                if(PlayerMovement.health <= 100) PlayerMovement.health += 25;
                if (PlayerMovement.health >= 100) return;
                print("Healed");
                Effects e = Instantiate(HealEffect, transform.position, Quaternion.identity);
                PlayerMovement.mana -= 25;
                PM.manaRegenTimer = 1;
                if (PlayerMovement.health >= 100) PlayerMovement.health = 100;
            }
            if (healingSpellCooldown >= 1) return;
            
        }
    }
}