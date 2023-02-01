using UnityEngine;
using UnityEngine.Events;

namespace SpyroClone.Items.Pickups
{
    public class PickUp : MonoBehaviour
    {
        public UnityEvent OnPickUp;

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PickUpItem();
                OnPickUp?.Invoke();
            }
        }

        public virtual void PickUpItem()
        {
            Destroy(gameObject);
        }
    }

}
