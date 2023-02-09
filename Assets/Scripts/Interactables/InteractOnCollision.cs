using System;
using SpyroClone.Utils;
using UnityEngine;

namespace SpyroClone.Interactables
{
    public class InteractOnCollision : MonoBehaviour
    {
        [SerializeField] LayerMask layersToCollideWith;
        [SerializeField] Interactable interactableToTrigger;

        private void OnTriggerEnter(Collider other)
        {

            if (layersToCollideWith.Contains(other.gameObject))
            {
                var interact = other.gameObject.GetComponent<InteractTriggerObject>();
                if (interact == null) { return; }

                interactableToTrigger.InteractWithObject(interact.GetInteractRequirement());
                interactableToTrigger.SetToTriggered();
            }
        }
    }
}