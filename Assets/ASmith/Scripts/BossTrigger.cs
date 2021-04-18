using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class BossTrigger : MonoBehaviour
    {
        BossController battleBegun;
        void Start()
        {

        }

        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                if (battleBegun == false)
                {
                    BossController.battleBegun = true;
                    Destroy(gameObject);
                }
            }
        }
    }
}
