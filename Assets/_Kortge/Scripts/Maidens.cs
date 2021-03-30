using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    /// <summary>
    /// The parent object of the maidens, used to ensure that only one rose is thrown at a time.
    /// </summary>
    public class Maidens : MonoBehaviour
    {
        /// <summary>
        /// Once the boss has been defeated, it represents how long the maidens will wait before throwing another rose.
        /// </summary>
        private float delay =0.2f;
        /// <summary>
        /// Checks this to see if it is sending out a signal to throw a rose.
        /// </summary>
        public Boss boss;
        /// <summary>
        /// All of the maidens that this script signals to throw a rose.
        /// </summary>
        public Maiden[] maidens = new Maiden[4];

        /// <summary>
        /// Checks if the boss is dead so that it can throw roses endlessly.
        /// </summary>
        // Update is called once per frame
        void Update()
        {
            if (boss.dead)
            {
                delay -= Time.deltaTime;
                if (delay <= 0)
                {
                    ThrowRose();
                    delay = 0.2f;
                }
            }
        }
        /// <summary>
        /// Signals one of the four maidens to throw a rose.
        /// </summary>
        public void ThrowRose()
        {
            Maiden maiden = maidens[Random.Range(0, 3)];
            maiden.ThrowRose();
        }
    }
}