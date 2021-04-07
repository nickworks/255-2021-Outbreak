using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Szczesniak {
    /// <summary>
    /// This class notifies player of UI
    /// </summary>
    public class UI_InfoElements : MonoBehaviour {

        /// <summary>
        /// amount of ammo
        /// </summary>
        public Text ammoAmount;
        
        /// <summary>
        /// ammo capacity
        /// </summary>
        private int ammoCapacity = 0;

        /// <summary>
        /// player's rife information
        /// </summary>
        public PlayerWeapon playerWeaponsInfo;


        /// <summary>
        /// Dash sprite image
        /// </summary>
        public Image dashImage;

        /// <summary>
        /// Information if the player dashed or not
        /// </summary>
        public PlayerMovement playerDashInfo;

        /// <summary>
        /// Rocket sprite image
        /// </summary>
        public Image rocketImage;

        void Start() {
            AmmoSetup(); // sets up ammo amount
        }

        void AmmoSetup() {
            ammoCapacity = playerWeaponsInfo.maxRoundsInClip; // stores ammo capacity's max
        }

        void Update() {
            // ammo informaiton that is displayed on the UI
            ammoAmount.text = playerWeaponsInfo.roundsInClip + " / " + ammoCapacity;

            RocketFade(); // fades rocket ability if used

            DashFade(); // fades dash ability if used
        }

        private void DashFade() {
            Color fadeOnDashImage = dashImage.color; // declaring color

            if (playerDashInfo.dashTimeToUseAgain <= 0) fadeOnDashImage.a = 1f; // if not used, stays normal color
            if (playerDashInfo.dashTimeToUseAgain > 0) fadeOnDashImage.a = .5f; // if used, lowers opacity

            dashImage.color = fadeOnDashImage; // sets color
        }

        void RocketFade() {
            Color fadeOnRocketImage = rocketImage.color; // declaring color

            if (playerWeaponsInfo.rocketTimer <= 0) fadeOnRocketImage.a = 1f; // if not used, stays normal color
            if (playerWeaponsInfo.rocketTimer > 0) fadeOnRocketImage.a = .5f; // if used, lowers opacity

            rocketImage.color = fadeOnRocketImage; // sets color
        }
    }
}