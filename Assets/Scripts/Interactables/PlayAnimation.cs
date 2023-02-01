using UnityEngine;

namespace SpyroClone.Interactables
{
    public class PlayAnimation : MonoBehaviour
    {
        [SerializeField] Animation[] clips;

        public void Interact()
        {
            foreach (var clip in clips)
            {
                clip.Play();
            }
        }
    }
}