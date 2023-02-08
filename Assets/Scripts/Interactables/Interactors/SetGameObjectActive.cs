using UnityEngine;

namespace SpyroClone.Interactables.Interactors
{
    public class SetGameObjectActive : Interactor
    {
        [SerializeField] GameObject[] gameObjects;
        [SerializeField] bool setActive;

        public override void Interact()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetActive(setActive);
            }
        }
    }
}