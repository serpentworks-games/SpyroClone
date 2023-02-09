using System.Collections.Generic;
using UnityEngine;

namespace SpyroClone.Inventories
{
    [CreateAssetMenu(menuName = "SpyroClone/New Item")]
    public class PickupItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] ItemType itemType;
        [SerializeField] PickUp pickup = null;
        [SerializeField] int value = 0;
        [SerializeField] string itemID = null;

        static Dictionary<string, PickupItem> itemLookUpCache;

        public static PickupItem GetFromID(string itemID)
        {
            if (itemLookUpCache == null)
            {
                itemLookUpCache = new Dictionary<string, PickupItem>();
                var itemList = Resources.LoadAll<PickupItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookUpCache.ContainsKey(item.itemID))
                    {
                        continue;
                    }

                    itemLookUpCache[item.itemID] = item;

                }
            }

            if (itemID == null || !itemLookUpCache.ContainsKey(itemID)) { return null; }
            return itemLookUpCache[itemID];
        }

        public PickUp SpawnPickup(Vector3 position)
        {
            var pickup = Instantiate(this.pickup);
            pickup.transform.position = position;
            pickup.Setup(this, itemType, value);
            return pickup;
        }

        public string GetItemID()
        {
            return itemID;
        }

        public ItemType GetItemType()
        {
            return itemType;
        }

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        public void OnBeforeSerialize()
        {

        }
    }
}