using UnityEngine;


namespace SpyroClone.Interactables.Interactors
{
    [RequireComponent(typeof(Interactable))]
    public abstract class Interactor : MonoBehaviour
    {
        [SerializeField] public InteractRequirement interactRequirement;
        [SerializeField] public bool isOneShot = true;
        [SerializeField] public float startDelay = 0;

        public bool isTriggered;

        public abstract void Interact();

        public virtual void OnInteract()
        {
            if (isTriggered == true && isOneShot) { return; }

            isTriggered = true;
            StartInteraction();
        }


        protected virtual void Awake()
        {
            GetComponent<Interactable>().Register(interactRequirement, this);
        }

        private void StartInteraction()
        {
            if (startDelay > 0)
            {
                Invoke("Interact", startDelay);
            }
            else
            {
                Interact();
            }
        }
    }
}