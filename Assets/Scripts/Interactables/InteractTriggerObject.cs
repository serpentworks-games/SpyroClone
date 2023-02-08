using UnityEngine;

namespace SpyroClone.Interactables
{
    public class InteractTriggerObject : MonoBehaviour
    {
        [SerializeField] public InteractRequirement interactRequirement;

        public InteractRequirement GetInteractRequirement()
        {
            return interactRequirement;
        }
    }
}