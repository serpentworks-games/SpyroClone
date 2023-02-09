using System.Collections.Generic;
using SpyroClone.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace SpyroClone.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        [SerializeField] float dropRadius = 0;
        [SerializeField] DropConfig[] drops;

        [System.Serializable]
        public class DropConfig
        {
            public PickupItem item;
            public int amountToDrop;
        }

        List<PickUp> droppedItems = new();

        const int kMaxAttempts = 30;

        public void Setup(DropConfig[] drops, float dropRadius)
        {
            this.drops = drops;
            this.dropRadius = dropRadius;
        }

        public void DropItems()
        {
            if (drops.Length == 0) { return; }

            foreach (var drop in drops)
            {
                for (int i = 0; i < drop.amountToDrop; i++)
                {   
                    Vector3 spawnLocation = GetDropLocation();
                    SpawnPickup(drop.item, spawnLocation);
                }
            }
        }

        public void DropItem(PickupItem item)
        {
            SpawnPickup(item, GetDropLocation());
        }

        private void SpawnPickup(PickupItem item, Vector3 spawnLocation)
        {
            var dropped = item.SpawnPickup(spawnLocation);
            droppedItems.Add(dropped);
        }

        Vector3 GetDropLocation()
        {
            if (dropRadius == 0)
            {
                return transform.position;
            }
            else
            {
                for (int i = 0; i < kMaxAttempts; i++)
                {
                    Vector3 randomPoint = transform.position + Random.insideUnitSphere * dropRadius;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        return hit.position;
                    }
                }
                return transform.position;
            }
        }

        [System.Serializable]
        struct SaveData
        {
            public string itemID;
            public SerializableVector3 position;
        }

        public object CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new SaveData[droppedItems.Count];
            for (int i = 0; i < droppedItemsList.Length; i++)
            {
                droppedItemsList[i].itemID = droppedItems[i].GetItem().GetItemID();
                droppedItemsList[i].position = new SerializableVector3(droppedItems[i].transform.position);
            }
            return droppedItemsList;
        }

        public void RestoreState(object state)
        {
            var droppedItemsList = (SaveData[])state;
            foreach (var item in droppedItemsList)
            {
                var pickupItem = PickupItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                SpawnPickup(pickupItem, position);
            }
        }

        void RemoveDestroyedDrops()
        {
            var newList = new List<PickUp>();
            foreach (var item in droppedItems)
            {
                if(item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }
    }
}
