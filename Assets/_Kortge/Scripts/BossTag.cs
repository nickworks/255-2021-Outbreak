using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    /// <summary>
    /// Makes this tag follow the boss.
    /// </summary>
    public class BossTag : MonoBehaviour
    {
        public Transform boss;
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {
            Vector3 newPosition;
            newPosition.y = boss.position.z;
            newPosition.x = boss.position.x;
            newPosition.z = 0;
            RectTransform position = GetComponent<RectTransform>();
            position.anchoredPosition = newPosition;
        }
    }
}