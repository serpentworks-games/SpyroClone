using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyroClone.Utils
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private void Update()
        {
            foreach (Transform child in transform)
            {
                if (!child.GetComponent<ParticleSystem>().IsAlive())
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}