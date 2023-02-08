using UnityEngine;
using UnityEngine.Events;

namespace SpyroClone.Interactables.Interactors
{
    public class ActivateParticleSystem : Interactor
    {
        [SerializeField] ParticleSystem[] particleSystems;

        public override void Interact()
        {
            foreach (var system in particleSystems)
            {
                system.Play();
            }
        }

    }
}