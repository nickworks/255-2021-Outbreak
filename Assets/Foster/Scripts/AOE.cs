using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Foster
{
    public class AOE : MonoBehaviour
    {

        private float lifeSpan = 3;
        private float age = 0;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            age += Time.deltaTime;

            if (age > lifeSpan)
            {
                Destroy(gameObject);
            }

        }
    }
}