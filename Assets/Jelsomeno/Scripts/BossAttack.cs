using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    public class BossAttack : MonoBehaviour
    {
        static class States
        {
            public class State
            {
                protected BossAttack attack;

                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(BossAttack attack)
                {
                    this.attack = attack;
                }
                virtual public void OnEnd()
                {

                }
            }

            public class Regular : State
            {
                public override State Update()
                {
                    if (attack.PlayerSeen(attack.PlayerTank, attack.viewingDis)) return new States.HeavyShot();

                    return null;
                }
            }

            public class HeavyShot : State
            {
                public override State Update()
                {
                    attack.HeavyShot();

                    if (!attack.PlayerSeen(attack.PlayerTank, attack.viewingDis)) return new States.Regular();

                    return null;
                }

            }

            public class Reload : State
            {
                float timetoReload = 10;

                public Reload(float reload)
                {
                    timetoReload = reload;
                }
            
                public override State Update()
                {
                   timetoReload -= Time.deltaTime;

                    if (timetoReload <= 0) return new States.Regular();

                    return null;
                
                }

            }

            public class Flamethrower : State
            {

            }

            public class Bombs : State
            {

            }

        }

        private States.State state;


        public Projectile HeavyShotBullet;
        public Transform bulletSpawn;
        public Transform PlayerTank;
        public GameObject impactEffect;
        public float viewingAng = 360;
        private float viewingDis = 40;
        public float ReloadTime = 7;

        private float roundsPerSec = 5;
        private float HeavyShotTimer = 0;
        private int TotalHeavyShots = 75;

        private Quaternion startingRot;

        // Start is called before the first frame update
        void Start()
        {
            startingRot = transform.localRotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (state == null) SwitchState(new States.Regular());

            if (state != null) SwitchState(state.Update());

            if (HeavyShotTimer > 0) HeavyShotTimer -= Time.deltaTime;

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
            if (HeavyShotTimer > 0) return;

            Projectile HS = Instantiate(HeavyShotBullet, transform.position, Quaternion.identity);
            HS.InitBullet(transform.forward * 20);

            TotalHeavyShots--;
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

    }

}
