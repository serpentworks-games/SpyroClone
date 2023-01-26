using SpyroClone.AI;
using UnityEngine;
using SpyroClone.Core;

namespace SpyroClone.Combat
{
    public class AICombat : MonoBehaviour, IAction
    {
        [Header("Attack Params")]
        [SerializeField] float attackRange = 1f;
        [SerializeField] float attackSpeed = 2f;
        [SerializeField] int attackDamage = 1;

        //Refs
        AIMovement movement;
        Animator animator;

        //State
        Damageable target;
        Transform targetLocation = null;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            movement = GetComponent<AIMovement>();
            animator = GetComponent<Animator>();

        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) { return; }
            if (target.GetIsDead()) { return; }

            if (!GetIsTargetInRange(target.transform))
            {
                movement.MoveToDestination(target.transform.position, 1f);
            }
            else
            {
                movement.Cancel();
                AttackBehaviour();
            }

        }

        public bool CanAttack(GameObject player)
        {
            if (player == null) { return false; }
            if (!movement.CanMoveToDest(player.transform.position)
                && !GetIsTargetInRange(target.transform)) { return false; }

            Damageable dmgToCheck = player.GetComponent<Damageable>();
            return dmgToCheck != null && !dmgToCheck.GetIsDead();
        }

        public void Attack(GameObject player)
        {
            target = player.GetComponent<Damageable>();
        }


        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);

            if (timeSinceLastAttack > attackSpeed)
            {
                animator.ResetTrigger("StopAttack");
                animator.SetTrigger("StartAttack");
                timeSinceLastAttack = 0;
            }
        }

        private bool GetIsTargetInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < attackRange;
        }

        public void Cancel()
        {
            target = null;
            animator.ResetTrigger("StartAttack");
            animator.SetTrigger("StopAttack");
            movement.Cancel();
        }

        //AnimEvents
        public void Hit()
        {
            if (target.GetIsDead()) { return; }
            target.ApplyDamage(attackDamage);
        }
    }
}