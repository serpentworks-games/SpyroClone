using System;
using SpyroClone.AI;
using SpyroClone.Combat;
using SpyroClone.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace SpyroClone.Core
{
    public class Damageable : MonoBehaviour, ISaveable
    {
        [SerializeField] int maxHealthPoints = 1;
        [SerializeField] float invunerableTime;
        [SerializeField] bool isInvulnerable;
        [SerializeField] bool isPlayer = false;
        [SerializeField] bool diesPermanantly = false;


        public UnityEvent OnTakeDamage, OnDeath, OnBecomeVulnerable, OnHitWhileInverable;

        //Refs
        Animator animator;

        NavMeshAgent agent;
        Collider col;

        //State
        int currentHitPoints;
        float timeSinceLastHit = Mathf.Infinity;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            col = GetComponent<Collider>();
        }

        private void Start()
        {
            ResetDamage();
        }

        private void Update()
        {
            if (isInvulnerable)
            {
                timeSinceLastHit += Time.deltaTime;
                if (timeSinceLastHit > invunerableTime)
                {
                    timeSinceLastHit = 0;
                    isInvulnerable = false;
                    OnBecomeVulnerable?.Invoke();
                }
            }
        }

        public bool GetIsDead()
        {
            return currentHitPoints <= 0;
        }


        public int GetCurrentHealth()
        {
            return currentHitPoints;
        }

        public void ApplyDamage(int damage)
        {
            if (GetIsDead()) { return; }

            if (isInvulnerable)
            {
                OnHitWhileInverable?.Invoke();
                return;
            }

            isInvulnerable = true;
            currentHitPoints -= damage;

            if (GetIsDead())
            {
                TriggerDeath();
                OnDeath?.Invoke();
            }
            else
            {
                OnTakeDamage?.Invoke();
            }
        }

        private void TriggerDeath()
        {
            if (!gameObject.CompareTag("Interactable"))
            {
                animator.SetTrigger("Death");
                if (gameObject.CompareTag("Enemy"))
                {
                    GetComponent<ActionScheduler>().CancelCurrentAction();
                    col.enabled = false;
                    GetComponent<AICombat>().enabled = false;
                }
            }
        }

        private void ResetDamage()
        {
            currentHitPoints = maxHealthPoints;
            isInvulnerable = false;
            timeSinceLastHit = Mathf.Infinity;
        }

        public object CaptureState()
        {
            return currentHitPoints;
        }

        public void RestoreState(object state)
        {
            int hp = (int)state;
            currentHitPoints = hp;
            if (diesPermanantly && currentHitPoints == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}