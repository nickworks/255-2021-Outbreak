using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class Maidens : MonoBehaviour
    {
        public Maiden[] maidens = new Maiden[4];
        public Boss boss;
        public PlayerAiming player;
        private float delay = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            delay -= Time.deltaTime;
            if (delay < 0) delay = 0;
            if ((boss.dead && delay == 0)|| (boss.maidenCue && !player.projectionReady))
            {
                Maiden maiden = maidens[Random.Range(0,3)];
                maiden.ThrowRose();
                delay = 1;
            }
            boss.maidenCue = false;
        }
    }
}