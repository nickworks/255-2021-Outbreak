using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class DestroyAfterSpawned : MonoBehaviour {

        void Start() {
            Destroy(this.gameObject, 3);
        }
    }
}