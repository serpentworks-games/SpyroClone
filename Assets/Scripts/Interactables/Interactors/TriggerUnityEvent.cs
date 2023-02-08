using UnityEngine;
using UnityEngine.Events;

namespace SpyroClone.Interactables.Interactors
{
    public class TriggerUnityEvent : Interactor
    {
        public UnityEvent OnTriggered;

        public override void Interact()
        {
            OnTriggered?.Invoke();
        }

    }
}