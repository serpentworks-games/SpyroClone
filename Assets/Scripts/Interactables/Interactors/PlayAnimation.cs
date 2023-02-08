using UnityEngine;

namespace SpyroClone.Interactables.Interactors
{
    public class PlayAnimation : Interactor
    {
        [SerializeField] Animation[] animations;

        public override void Interact()
        {
            foreach (var clip in animations)
            {
                clip.Play();
            }
        }
    }
}