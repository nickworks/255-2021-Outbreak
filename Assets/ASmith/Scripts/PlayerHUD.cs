using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ASmith
{
		public class PlayerHUD : MonoBehaviour
	{
        #region Ammo Variables
        /// <summary>
        /// References ammo icon used in the ammo bar on the HUD
        /// </summary>
        public Image ammo1;
		public Image ammo2;
		public Image ammo3;
		public Image ammo4;
		public Image ammo5;
		public Image ammo6;
		public Image ammo7;
		public Image ammo8;
		public Image ammo9;
		public Image ammo10;

        /// <summary>
        /// References the text box that displays the current ammo in clip
        /// </summary>
        public Text ammoCount;
        #endregion

        /// <summary>
        /// References text box used to display dashes available
        /// </summary>
        public Text dashCount;

        void Start()
		{

		}

		void Update()
		{
			RenderAmmoCount();
            //dashCount.text = PlayerMovement.dashCounter.ToString();

            if (PlayerMovement.dashCounter == 2) { dashCount.text = "2"; } 
            else if (PlayerMovement.dashCounter == 1) { dashCount.text = "1"; }
            else if (PlayerMovement.dashCounter == 0) { dashCount.text = "0"; }
        }

		private void RenderAmmoCount() // Handles displaying how much ammo is available to the player
		{
            if (PlayerWeapon.roundsInClip == 9) { ammo10.enabled = false; } // If 9 rounds in clip, turn off ammo10
            else if (PlayerWeapon.roundsInClip == 8) { ammo9.enabled = false; } // If 8 rounds in clip, turn off ammo9
            else if (PlayerWeapon.roundsInClip == 7) { ammo8.enabled = false; } // If 7 rounds in clip, turn off ammo8
            else if (PlayerWeapon.roundsInClip == 6) { ammo7.enabled = false; } // If 6 rounds in clip, turn off ammo7
            else if (PlayerWeapon.roundsInClip == 5) { ammo6.enabled = false; } // If 5 rounds in clip, turn off ammo6
            else if (PlayerWeapon.roundsInClip == 4) { ammo5.enabled = false; } // If 4 rounds in clip, turn off ammo5
            else if (PlayerWeapon.roundsInClip == 3) { ammo4.enabled = false; } // If 3 rounds in clip, turn off ammo4
            else if (PlayerWeapon.roundsInClip == 2) { ammo3.enabled = false; } // If 2 rounds in clip, turn off ammo3
            else if (PlayerWeapon.roundsInClip == 1) { ammo2.enabled = false; } // If 1 rounds in clip, turn off ammo2
            else if (PlayerWeapon.roundsInClip == 0) { ammo1.enabled = false; } // If 0 rounds in clip, turn off ammo1

            if (PlayerWeapon.roundsInClip < 10 && Input.GetButton("Reload")) // If there are less than 10 rounds in clip and player presses R...
            {
                // ...Enable all ammo icons on HUD
                ammo1.enabled = true;
                ammo2.enabled = true;
                ammo3.enabled = true;
                ammo4.enabled = true;
                ammo5.enabled = true;
                ammo6.enabled = true;
                ammo7.enabled = true;
                ammo8.enabled = true;
                ammo9.enabled = true;
                ammo10.enabled = true;
            }

            ammoCount.text = PlayerWeapon.roundsInClip.ToString(); // Displays available ammo in number form on HUD
        }
	}
}

