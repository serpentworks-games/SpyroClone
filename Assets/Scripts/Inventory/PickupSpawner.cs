using System;
using SpyroClone.Saving;
using UnityEngine;

namespace SpyroClone.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] PickupItem item = null;

        private void Awake()
        {
            SpawnPickup();
        }

        public PickUp GetPickUp()
        {
            return GetComponentInChildren<PickUp>();
        }

        public bool HasBeenPickedUp()
        {
            return GetPickUp() == null;
        }

        private void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if(GetPickUp())
            {
                Destroy(GetPickUp().gameObject);
            }
        }

        public object CaptureState()
        {
            return HasBeenPickedUp();
        }

        public void RestoreState(object state)
        {
            bool shouldBePickedUp = (bool)state;

            if(shouldBePickedUp && !HasBeenPickedUp())
            {
                DestroyPickup();
            }

            if(!shouldBePickedUp && HasBeenPickedUp())
            {
                SpawnPickup();
            }
        }
    }
}