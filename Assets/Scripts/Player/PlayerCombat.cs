using SpyroClone.Combat;
using SpyroClone.Core;
using UnityEngine;

namespace SpyroClone.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] Transform projectileLaunchPoint;

        Animator animator;
        TargetTracker targetTracker;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            targetTracker = GetComponent<TargetTracker>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Attack1");
                Debug.Log("Basic Attack!");
            }
        }

        //AnimEvent
        public void Launch()
        {
            Collider activeTarget = targetTracker.GetActiveTarget();
            if (activeTarget == null)
            {
                CreateProjectile(null);
                return;
            }
            Damageable target = activeTarget.GetComponent<Damageable>();
            CreateProjectile(target);
        }

        private void CreateProjectile(Damageable target)
        {
            Projectile instance = Instantiate(projectilePrefab, projectileLaunchPoint.position, transform.rotation);
            instance.Setup(target);
        }
    }
}