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

	    void Update()
	    {
	        RenderAmmoCount();

            // Logic for communicating the player's dash count to the UI
            if (PlayerMovement.dashCounter == 2) { dashCount.text = "2"; } 
            else if (PlayerMovement.dashCounter == 1) { dashCount.text = "1"; }
            else if (PlayerMovement.dashCounter == 0) { dashCount.text = "0"; }
        }

	    private void RenderAmmoCount() // Handles displaying how much ammo is available to the player
	    {
            ammo10.enabled = (PlayerWeapon.roundsInClip >= 10); //{ ammo10.enabled = false; } // If 9 rounds in clip, turn off ammo10
            ammo9.enabled = (PlayerWeapon.roundsInClip >= 9); //{ ammo9.enabled = false; } // If 8 rounds in clip, turn off ammo9
            ammo8.enabled = (PlayerWeapon.roundsInClip >= 8); //{ ammo8.enabled = false; } // If 7 rounds in clip, turn off ammo8
            ammo7.enabled = (PlayerWeapon.roundsInClip >= 7); //{ ammo7.enabled = false; } // If 6 rounds in clip, turn off ammo7
            ammo6.enabled = (PlayerWeapon.roundsInClip >= 6); //{ ammo6.enabled = false; } // If 5 rounds in clip, turn off ammo6
            ammo5.enabled = (PlayerWeapon.roundsInClip >= 5); //{ ammo5.enabled = false; } // If 4 rounds in clip, turn off ammo5
            ammo4.enabled = (PlayerWeapon.roundsInClip >= 4); //{ ammo4.enabled = false; } // If 3 rounds in clip, turn off ammo4
            ammo3.enabled = (PlayerWeapon.roundsInClip >= 3); //{ ammo3.enabled = false; } // If 2 rounds in clip, turn off ammo3
            ammo2.enabled = (PlayerWeapon.roundsInClip >= 2); //{ ammo2.enabled = false; } // If 1 rounds in clip, turn off ammo2
            ammo1.enabled = (PlayerWeapon.roundsInClip >= 1); //{ ammo1.enabled = false; } // If 0 rounds in clip, turn off ammo1

            ammoCount.text = PlayerWeapon.roundsInClip.ToString(); // Displays available ammo in number form on HUD
        }
	}
}

