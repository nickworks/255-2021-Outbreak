using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hodgkins
{
    public class GUIController : MonoBehaviour
    {
        public Text numAmmo;
        public Text playerHealthDisplay;
        public Text bossHealthDisplay;


        private int numAmmoMax = 0;


        public PlayerWeapon pw;
        public HealthSystem playerHealth;
        public HealthSystem bossHealth;



        void Start()
        {
            
        }


        // Update is called once per frame
        void Update()
        {
            numAmmo.text = pw.roundsInClip + " / ";
            playerHealthDisplay.text = playerHealth.health + "%";
            //updatedBossHealth = bossHealth / 10;
            bossHealthDisplay.text = bossHealth.health + "%";

            if (playerHealth.health <= 0)
            {
                Outbreak.Game.GameOver();
            }

            if (bossHealth.health <= 0)
            {
                Outbreak.Game.GotoNextLevel();
            }
        }
    }
}