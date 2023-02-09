using System;
using UnityEngine;
using UnityEngine.Events;

namespace SpyroClone.Inventories
{
    public class PickUp : MonoBehaviour
    {
        [SerializeField] UnityEvent OnPickUp;

        int value;
        PickupItem item;
        ItemType itemType;

        Inventory inventory;

        private void Awake() {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OnPickUp?.Invoke();
                PickUpItem();
            }
        }

        public void Setup(PickupItem item, ItemType itemType, int value)
        {
            this.item = item;
            this.itemType = itemType;
            this.value = value;
        }

        public PickupItem GetItem()
        {
            return item;
        }

        public ItemType GetItemType()
        {
            return itemType;
        }

        public void PickUpItem()
        {
            switch (itemType)
            {
                case ItemType.Gem:
                    inventory.AddGems(value);
                    break;
                case ItemType.BasicKey:
                    inventory.AddBasicKey(value);
                    break;
                case ItemType.BossKey:
                    inventory.AddBossKey(value);
                    break;
            }
            Destroy(gameObject);
        }
    }

}
