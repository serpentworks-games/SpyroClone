using System;
using System.Collections.Generic;
using SpyroClone.Interactables.Interactors;
using UnityEngine;

namespace SpyroClone.Interactables
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] public InteractRequirement interactRequirement;

        public bool respawns;
        
        Dictionary<InteractRequirement, List<System.Action>> handlers = new();

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
            if(!handlers.TryGetValue(interactRequirement, out callbacks))
            {
                callbacks = handlers[interactRequirement] = new();
            }

            callbacks.Add(interactor.OnInteract);
        }

        public void Remove(InteractRequirement interactRequirement, Interactor interactor)
        {
            handlers[interactRequirement].Remove(interactor.OnInteract);
        }
    }
}