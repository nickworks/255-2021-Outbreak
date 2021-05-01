using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Velting
{
    public class UIManager : MonoBehaviour
    {
        public Text playerHealth;

        public PlayerMovement playerInfo;

        public Text bossHealth;

        public EnemyBossController bossInfo;

        public Text ammoAmount;    

        public PlayerWeapon ammoInfo;

        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            
                ammoAmount.text = "Ammo: " + ammoInfo.roundsInClip + " / " + ammoInfo.maxRoundsInClip;
                bossHealth.text = "Boss:" + bossInfo.health + " health";
                playerHealth.text = "Player:" + playerInfo.health + " health";
            

            
        }

        

        

        
    }
}