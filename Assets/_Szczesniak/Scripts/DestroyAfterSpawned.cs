using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class is to destroy objects that spawn and are not needed after a few seconds
    /// </summary>
    public class DestroyAfterSpawned : MonoBehaviour {

        void Start() {
            Destroy(this.gameObject, 3); // destroy gameObject at 3 seconds
        }
    }
}