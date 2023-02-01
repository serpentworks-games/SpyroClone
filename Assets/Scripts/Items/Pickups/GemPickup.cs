using System.Collections;
using System.Collections.Generic;
using SpyroClone.Items;
using UnityEngine;

namespace SpyroClone.Items.Pickups
{
    public class GemPickup : PickUp
    {
        [SerializeField] int gemAmount = 20;

        Gems gems; 

        private void Awake() {
            gems = GameObject.FindWithTag("Player").GetComponent<Gems>();
        }

        public override void PickUpItem()
        {
            if (gems == null) { return; }

            gems.AddGems(gemAmount);

            base.PickUpItem();
        }
    }
}