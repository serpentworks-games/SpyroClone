using System;
using System.Collections.Generic;
using SpyroClone.Interactables.Interactors;
using SpyroClone.Inventories;
using SpyroClone.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace SpyroClone.Interactables
{
    public class Interactable : MonoBehaviour, ISaveable
    {
        [SerializeField] public InteractRequirement interactRequirement;
        [SerializeField] UnityEvent OnSetup;

        public void Setup()
        {
            if (!hasBeenTriggered) { return; }

            OnSetup?.Invoke();
        }

        Dictionary<InteractRequirement, List<System.Action>> handlers = new();

        public bool hasBeenTriggered;

        public void SetToTriggered()
        {
            hasBeenTriggered = true;
        }

        public virtual void CheckConditions(Inventory inventory)
        {
            
        }

        public void InteractWithObject(InteractRequirement interactReq)
        {
            List<Action> callbacks;
            if (handlers.TryGetValue(interactReq, out callbacks))
            {
                foreach (var item in callbacks)
                {
                    item();
                }
            }
        }

        public void Register(InteractRequirement interactRequirement, Interactor interactor)
        {
            List<Action> callbacks;
            if (!handlers.TryGetValue(interactRequirement, out callbacks))
            {
                callbacks = handlers[interactRequirement] = new();
            }

            callbacks.Add(interactor.OnInteract);
        }

        public void Remove(InteractRequirement interactRequirement, Interactor interactor)
        {
            handlers[interactRequirement].Remove(interactor.OnInteract);
        }

        public virtual object CaptureState()
        {
            Debug.Log($"Saving hasBeenTriggered as {hasBeenTriggered} for {gameObject.name}");
            return hasBeenTriggered;
        }

        public virtual void RestoreState(object state)
        {
            bool data = (bool)state;
            hasBeenTriggered = data;
            Debug.Log($"Restoring hasBeenTriggered to {data} for {gameObject.name}");
            Setup();
        }
    }
}