using System.Collections;
using System.Collections.Generic;
using SpyroClone.Core;
using TMPro;
using UnityEngine;

namespace SpyroClone.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI healthValue;

        Damageable damageable;

        private void Awake() {
            damageable = GameObject.FindWithTag("Player").GetComponent<Damageable>();
        }

        // Update is called once per frame
        void Update()
        {
            healthValue.text = damageable.GetCurrentHealth().ToString();
        }
    }
}