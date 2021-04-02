using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Szczesniak {
    public class BulletAmount : MonoBehaviour {

        public Text ammoAmount;
        private int ammoCapacity = 0;
        public PlayerWeapon ammoInfo;

        void Start() {
            AmmoSetup();
        }

        void AmmoSetup() {
            ammoCapacity = ammoInfo.maxRoundsInClip;
        }

        void Update() {
            ammoAmount.text = ammoInfo.roundsInClip + " / " + ammoCapacity;
        }
    }
}