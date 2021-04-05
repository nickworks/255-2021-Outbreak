using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Szczesniak {
    public class UI_InfoElements : MonoBehaviour {

        public Text ammoAmount;
        private int ammoCapacity = 0;
        public PlayerWeapon playerWeaponsInfo;

        public Image dashImage;
        public PlayerMovement playerDashInfo;

        public Image rocketImage;

        void Start() {
            AmmoSetup();
        }

        void AmmoSetup() {
            ammoCapacity = playerWeaponsInfo.maxRoundsInClip;
        }

        void Update() {
            ammoAmount.text = playerWeaponsInfo.roundsInClip + " / " + ammoCapacity;
            
            RocketFade();

            Color fadeOnDashImage = dashImage.color;

            if (playerDashInfo.dashTimeToUseAgain <= 0) fadeOnDashImage.a = 1f;
            if (playerDashInfo.dashTimeToUseAgain > 0) fadeOnDashImage.a = .5f;

            dashImage.color = fadeOnDashImage;
        }

        void RocketFade() {
            Color fadeOnRocketImage = rocketImage.color;

            if (playerWeaponsInfo.rocketTimer <= 0) fadeOnRocketImage.a = 1f;
            if (playerWeaponsInfo.rocketTimer > 0) fadeOnRocketImage.a = .5f;

            rocketImage.color = fadeOnRocketImage;
        }
    }
}