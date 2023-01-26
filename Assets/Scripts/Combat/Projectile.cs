using SpyroClone.Core;
using SpyroClone.Utils;
using UnityEngine;

namespace SpyroClone.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] int projDamage = 1;
        [SerializeField] float projSpeed = 5f;
        [SerializeField] float maxLifetime = 5f;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] LayerMask layersToCollideWith;
        [SerializeField] GameObject hitEffect;

        Damageable target;

        private void Update()
        {
            if (target != null)
            {
                transform.LookAt(GetAimLocation());
            }

            transform.Translate(Vector3.forward * projSpeed * Time.deltaTime);
        }

        public void Setup(Damageable target)
        {
            this.target = target;
            Destroy(gameObject, maxLifetime);
        }

        private Vector3 GetAimLocation()
        {
            if (target.GetComponent<CapsuleCollider>() == null)
            {
                return target.transform.position;
            }
            return target.GetComponent<CapsuleCollider>().center;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) { return; }

            if (layersToCollideWith.Contains(other.gameObject))
            {
                if (other.GetComponent<Damageable>())
                {
                    other.GetComponent<Damageable>().ApplyDamage(projDamage);
                }
                if(hitEffect != null)
                {
                    Instantiate(hitEffect, transform.position, transform.rotation);
                }
                projSpeed = 0;
                Destroy(gameObject, lifeAfterImpact);
            }

        }
    }
}