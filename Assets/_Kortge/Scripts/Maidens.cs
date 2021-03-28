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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            int maidenIndex = 0;
            List<int> maidenIndexes = new List<int>();
            foreach (Maiden maiden in maidens)
            {
                if (!maiden.roseThrown) maidenIndexes.Add(maidenIndex);
                maidenIndex++;
            }
            if (boss.maidenCue && maidenIndexes.Count > 0 && player.roses < 6)
            {
                Maiden maiden = maidens[maidenIndexes[Random.Range(0, maidenIndexes.Count - 1)]];
                maiden.ThrowRose();
            }
            boss.maidenCue = false;
        }
    }
}