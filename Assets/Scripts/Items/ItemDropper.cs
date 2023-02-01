using System.Collections;
using System.Collections.Generic;
using SpyroClone.Items.Pickups;
using UnityEngine;
using UnityEngine.AI;

namespace SpyroClone.Items
{
    public class ItemDropper : MonoBehaviour
    {
        [SerializeField] float dropRadius = 0;
        [SerializeField] DropConfig[] drops;

        [System.Serializable]
        public class DropConfig
        {
            public PickUp item;
            public int amountToDrop;
        }

        const int kMaxAttempts = 30;

        public void DropItems()
        {
            if (drops.Length == 0) { return; }
            foreach (var drop in drops)
            {
                for (int i = 0; i < drop.amountToDrop; i++)
                {
                    Instantiate(drop.item, GetDropLocation(), Quaternion.identity);
                }
            }
        }

        Vector3 GetDropLocation()
        {
            if (dropRadius == 0)
            {
                return transform.position;
            }
            else
            {
                for (int i = 0; i < kMaxAttempts; i++)
                {
                    Vector3 randomPoint = transform.position + Random.insideUnitSphere * dropRadius;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                    {
                        return hit.position;
                    }
                }
                return transform.position;
            }
        }
    }
}
