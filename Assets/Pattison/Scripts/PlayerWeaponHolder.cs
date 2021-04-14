using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pattison {
    public class PlayerWeaponHolder : MonoBehaviour {

        /// <summary>
        /// The current weapon in the player's hands.
        /// </summary>
        private PlayerWeapon currentWeapon;

        /// <summary>
        /// The weapons in the player's inventory.
        /// </summary>
        public List<PlayerWeapon> heldWeapons = new List<PlayerWeapon>();

        /// <summary>
        /// The index number of the currentWeapon within the heldWeapons List<>.
        /// </summary>
        private int weaponIndex = 0;

        void Start() {
            EquipCurrentWeapon();
        }

        private void Update() {
            
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

            if (scroll < 0) PrevWeapon();
            if (scroll > 0) NextWeapon();

        }

        public void NextWeapon() {
            weaponIndex++;
            EquipCurrentWeapon();
        }
        public void PrevWeapon() {
            weaponIndex--;
            EquipCurrentWeapon();
        }

        void EquipCurrentWeapon() {

            if (heldWeapons.Count == 0) return;

            if (weaponIndex < 0) weaponIndex = heldWeapons.Count - 1;
            if (weaponIndex >= heldWeapons.Count) weaponIndex = 0;

            PlayerWeapon weaponToSpawn = heldWeapons[weaponIndex];

            if (weaponToSpawn == null) return; // abort!

            if (currentWeapon) {
                Destroy(currentWeapon.gameObject); // destroy existing weapon
            }

            currentWeapon = Instantiate(weaponToSpawn, transform);


        }


    }
}