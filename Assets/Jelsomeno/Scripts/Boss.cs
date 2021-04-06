using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Jelsomeno
{
    public class Boss : MonoBehaviour
    {

        static class States
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


            public class Roaming : State
            {
                /*
                public override State Update()
                {
                    


                    return null;
                }
                */
            }

            public class ArmorHealth : State
            {

            }

            public class HeavyShot : State
            {
                public override State Update()
                {
                    boss.HeavyShot();

                    return null;
                }

            }

            public class Reload : State
            {

            }

            public class Flamethrower : State
            {

            }

            public class Bombs : State
            {

            }
        
        }

        public Projectile HeavyShotBullet;
        public Transform bulletSpawn;
        //public Transform PartToRotate;
        //public string playerTag = "Player";
        public float range = 40;
        public float viewingAng = 360;
        public Transform PlayerTank;
        public float ReloadTime = 7;

        private float roundsPerSec = 5;
        private float HeavyShotTimer = 0;
        private int TotalHeavyShots = 75;
        private States.State state;
        private NavMeshAgent nav;
        private Quaternion startingRot;



        // Start is called before the first frame update
        void Start()
        {
           nav = GetComponent<NavMeshAgent>();

           startingRot = transform.localRotation;
        }

        // Update is called once per frame
        private void Update()
        {
            if (state == null) SwitchState(new States.Roaming());

            if (state != null) SwitchState(state.Update());

            if(HeavyShotTimer > 0) HeavyShotTimer -= Time.deltaTime;

            if (PlayerTank != null) nav.SetDestination(PlayerTank.position);
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;


            if (state != null) state.OnEnd();
            state = newState;
            state.OnStart(this);
        }

        void HeavyShot()
        {
            if (ReloadTime > 0) return;

            Projectile heavyShots = Instantiate(HeavyShotBullet, bulletSpawn.position, Quaternion.identity);
            heavyShots.InitBullet(transform.forward * 20);

            HeavyShotTimer = 1 / roundsPerSec;


        }


        private bool PlayerSeen(Transform thing, float visibleDis)
        {
            if (!thing) return false; // player is not visible and immmediately returns

            Vector3 vToThing = thing.position - transform.position;

            // checking the distance away from player
            if (vToThing.sqrMagnitude > visibleDis * visibleDis)
            {
                return false;
            }

            // checking it surrounding to see if player is withing its vision (360), then if not returning false
            if (Vector3.Angle(transform.forward, vToThing) > viewingAng) return false; 

            return true;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }

    }

}
