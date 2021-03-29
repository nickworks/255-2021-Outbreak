using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class AfterImage : MonoBehaviour
    {
        private SpriteRenderer sprite;
        private float t;
        private float activeTime;
        public float destroyTime;

        // Start is called before the first frame update
        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            activeTime += Time.deltaTime;
            t = activeTime / destroyTime;
            sprite.color = Color.Lerp(Color.red, Color.clear, t);
            if (t >= 1) Destroy(gameObject);
        }
    }
}