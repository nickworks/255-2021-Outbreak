using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Outbreak {
    public class MainMenu : MonoBehaviour {

        public Transform menuMain;
        public Transform menuWarp;

        public EventSystem events;

        void Start() {
            if (events == null) events = GameObject.FindObjectOfType<EventSystem>();
        }
        public void BttnPlay() {
            SceneManager.LoadScene("SceneSwitcher");
        }
        public void BttnShowWarpMenu() {
            menuMain.gameObject.SetActive(false);
            menuWarp.gameObject.SetActive(true);
            events.SetSelectedGameObject(null);
        }
        public void BttnHideWarpMenu() {
            menuWarp.gameObject.SetActive(false);
            menuMain.gameObject.SetActive(true);
            events.SetSelectedGameObject(events.firstSelectedGameObject);
        }
        public void BttnExit() {
            Application.Quit();
        }
    }
}