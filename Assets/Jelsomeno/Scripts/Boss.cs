using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            }

            public class Pursue : State
            {

            }

            public class SpeedUP : State
            {

            }

            public class AttackinState : State
            {

            }

            public class ReloadStates : State
            {

            }

            public class HeavyShot : State
            {

            }

            public class LightShot : State
            {

            }

            public class Health : State
            {

            }


        
        }

        //public Projectile prefabHeavyShot;

        //public Projectile prebabLightShot;

        public Transform attackPlayerTank;

        //private NavMeshAgent nav;

        private States.State state;




        // Start is called before the first frame update
        void Start()
        {
           //nav = GetComponent<NavMeshAgent>();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
