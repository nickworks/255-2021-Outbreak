using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Outbreak {
    public class GameOverMenu : MonoBehaviour {

        public void Start() {
            Cursor.visible = true;
        }
        public void BttnBackToMainMenu() {
            SceneManager.LoadScene("MainMenu");
        }
        public void BttnBackToPlaying() {
            SceneManager.LoadScene("SceneSwitcher");
        }
    }
}