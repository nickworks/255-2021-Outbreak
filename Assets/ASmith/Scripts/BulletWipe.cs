using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ASmith
{
    public class BulletWipe : MonoBehaviour
    {
        /// <summary>
        /// Variable that communcates the amount of bullet wipes available to the UI
        /// </summary>
        public Text wipeCount;

        /// <summary>
        /// Variable that tracks the amount of bullet wipes available
        /// </summary>
        public float bulletWipes = 3;

        void Start()
        {
            wipeCount.text = "3"; // Tells the UI that there are 3 bullet wipes available at the start of the game
        }

        void Update()
        {
            if (Input.GetButtonDown("BulletWipe") && bulletWipes > 0) // If player presses "E"...
            {
                bulletWipes--; // Subtract 1 bullet wipe from wipeCount
                wipeCount.text = bulletWipes.ToString(); // Communicates amount of bullet wipes to the UI
                GameObject[] BadBullets = GameObject.FindGameObjectsWithTag("BadBullet"); // Gets a reference to the BadBullets in the scene
                SoundBoard.PlayPlayerWipe(); // Plays the bullet wipe sound

                foreach (GameObject BadBullet in BadBullets) // for each bad bullet in the scene...
                { 
                    Destroy(BadBullet); // Destroy the bad bullets
                }
            }
            else if (Input.GetButtonDown("BulletWipe") && bulletWipes <= 0) // If no bullet wipes available
            {
                SoundBoard.PlayPlayerNoAmmo(); // // Play the no ammo sound
            }
        }
    }
}