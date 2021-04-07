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
                float roamTime = 7;

                public Roaming(float time)
                {
                    roamTime = time;
                }

                public override State Update()
                {
                    if (boss.PlayerSeen(boss.PlayerTank, true, boss.viewingDis)) return new States.AttackPlayer();


                    return null;
                }
                
            }

            public class AttackPlayer : State
            {
                public override State Update()
                {
                    boss.MoveTowardsPlayer();

                    if (!boss.PlayerSeen(boss.PlayerTank, true, boss.viewingDis)) return new States.Roaming(boss.timeToStop);

                    return null;
                }
            }

            public class Death : State
            {

            }

        
        }



        private States.State state;

        private NavMeshAgent nav;

        public Transform PlayerTank;

        public float viewingDis = 10;
        public float viewingAng = 35;
        public float stopDis = 25;
        public float InRange = 15;
        private float timeToStop = 5;




        // Start is called before the first frame update
        void Start()
        {
           nav = GetComponent<NavMeshAgent>();

        }

        // Update is called once per frame
        private void Update()
        {
            if (state == null) SwitchState(new States.Roaming(stopDis));

            if (state != null) SwitchState(state.Update());

            print(state);

        }

        void MoveTowardsPlayer()
        {
            if (PlayerTank != null) nav.SetDestination(PlayerTank.position);
        }

        void SwitchState(States.State newState)
        {
            if (newState == null) return;


            if (state != null) state.OnEnd();
            state = newState;
            state.OnStart(this);
        }

        private bool PlayerSeen(Transform thing, bool vision, float shooting)
        {
            if (!thing) return false; // player is not visible and immmediately returns

            Vector3 vToThing = thing.position - transform.position;

            if (vToThing.sqrMagnitude > shooting * shooting)
            {
                if (vision) stopMoving();

                return false;
            }

            if (Vector3.Angle(transform.forward, vToThing) > viewingAng) return false;

            if( vToThing.sqrMagnitude < shooting * (shooting - stopDis))
            {
                stopMoving();
            }
            else
            {
                ContinueMovement();
            }

            return true;
        }

        void stopMoving()
        {
            nav.updatePosition = false;
            nav.nextPosition = gameObject.transform.position;
        }

        void ContinueMovement()
        {
            nav.updatePosition = true;
        }
    }

}
