using SpyroClone.Combat;
using SpyroClone.Saving;
using UnityEngine;

namespace SpyroClone.Interactables
{
    public class GemChest : Interactable, ISaveable
    {
        bool hasBeenTriggered;

        public void SetToTriggered()
        {
            hasBeenTriggered = true;
        }

        public object CaptureState()
        {
            Debug.Log("Saving!");
            return hasBeenTriggered;
        }

        public void RestoreState(object state)
        {
            bool data = (bool)state;
            hasBeenTriggered = data;
            //Debug.Log("Restoring!");
            if (respawns == false && hasBeenTriggered)
            {
                gameObject.SetActive(false);
            }
        }
    }

}