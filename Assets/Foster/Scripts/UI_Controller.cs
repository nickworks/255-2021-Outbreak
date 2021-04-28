using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Foster
{
    public class UI_Controller : MonoBehaviour
    {
        public Scrollbar healthBar;
        public Scrollbar manaBar;
        public Scrollbar bossBar;

        void Update()
        {
            healthBar.size = (PlayerMovement.health / 100);
            manaBar.size = (PlayerMovement.mana / 100);
            bossBar.size = (EnemyBasicController.health / 100);
        }
    }
}