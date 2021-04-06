using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    
    /// <summary>
    /// This class if for when the player does certain action it will play sound files
    /// </summary>
    public class SoundEffectBoard : MonoBehaviour {

        /// <summary>
        /// This is a Singleton!
        /// </summary>
        public static SoundEffectBoard main;

        /// <summary>
        /// When the player jumps
        /// </summary>
        public AudioClip shooting;

        /// <summary>
        /// Sound when the player dies
        /// </summary>
        public AudioClip soundDie;

        /// <summary>
        /// Sound when the plaeyr picks up coins
        /// </summary>
        public AudioClip rocket;

        /// <summary>
        /// Sound when the player hurts themselve
        /// </summary>
        public AudioClip playerHurt;

        /// <summary>
        /// Sound when the player overlaps with a boast object
        /// </summary>
        public AudioClip soundDash;

        /// <summary>
        /// Sound when the player picks up a power up
        /// </summary>
        public AudioClip winSound;

        /// <summary>
        /// Creating audio source to play sounds on
        /// </summary>
        private AudioSource player;

        // Start is called before the first frame update
        void Start() {

            if (main == null) {
                main = this;
                player = GetComponent<AudioSource>();
            } else {
                Destroy(this.gameObject);
            }

        }

        /// <summary>
        /// Plays when the player jumps at a specific point in the world
        /// </summary>
        /// <param name="pos"></param>
        public static void PlayJump(Vector3 pos) {
            AudioSource.PlayClipAtPoint(main.shooting, pos);
        }

        /// <summary>
        /// Plays when the player jumps
        /// </summary>
        public static void PlayJump2() {
            main.player.PlayOneShot(main.shooting);
        }

        /// <summary>
        /// Plays the coin pickup sound file
        /// </summary>
        public static void PlayCoinPickup() {
            main.player.PlayOneShot(main.rocket);
        }

        /// <summary>
        /// Plays the sound death file when the player dies
        /// </summary>
        public static void PlayDeathSound() {
            main.player.PlayOneShot(main.soundDie);
        }

        /// <summary>
        /// Plays the sound when the player takes damage
        /// </summary>
        public static void PlayerDamaged() {
            main.player.PlayOneShot(main.playerHurt);
        }

        /// <summary>
        /// Plays when the player is overlapping the boast objects
        /// </summary>
        public static void BoastSound() {
            main.player.PlayOneShot(main.soundDash);
        }

        /// <summary>
        /// Plays when the player picks up the power up items
        /// </summary>
        public static void PowerUpSound() {
            main.player.PlayOneShot(main.winSound);
        }
    }
}