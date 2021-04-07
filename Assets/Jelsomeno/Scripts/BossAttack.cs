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
        public float viewingAng = 360;
        private float viewingDis = 40;
        public float ReloadTime = 7;

        private float roundsPerSec = 5;
        private float HeavyShotTimer = 0;
        private int TotalHeavyShots = 75;

        private Quaternion startingRot;

        public bool lockRotationX;

        public bool lockRotationY;

        public bool lockRotationZ;


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

        private void PointAt()
        {
            if (PlayerTank)
            {
                Vector3 disToTarget = PlayerTank.position - transform.position; // Gets distance

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
            }
            else
            {
                // figure out bone rotation, no target:

                transform.localRotation = AnimMath.Slide(transform.localRotation, startingRot, .05f);
            }
        }

        void HeavyShot()
        {
            if (HeavyShotTimer > 0) return;

            Projectile p = Instantiate(HeavyShotBullet, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

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
