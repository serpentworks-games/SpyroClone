using System;
using SpyroClone.Inventories;
using SpyroClone.Utils;
using UnityEngine;

namespace SpyroClone.Interactables
{
    public class InteractOnTriggerCol : MonoBehaviour
    {
        [SerializeField] LayerMask layersToCollideWith;
        [SerializeField] Interactable interactableToTrigger;

        Inventory inventory;

        private void Awake()
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (layersToCollideWith.Contains(other.gameObject))
            {
                interactableToTrigger.CheckConditions(inventory);
            }
        }
    }
}