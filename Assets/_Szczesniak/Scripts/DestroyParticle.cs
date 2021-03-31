using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class DestroyParticle : MonoBehaviour {


        void Start() {
            Destroy(this.gameObject, 3);
        }


    }
}