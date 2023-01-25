using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.Combat;
using SpyroClone.DamageSystem;
using UnityEngine;

namespace SpyroClone.AI.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Base Enemy Params")]
        [SerializeField] float chaseDistance = 5;
        [SerializeField] float suspicionTime = 2f;

        [Header("Patrol Behaviour")]
        [SerializeField] float randomPointDwellTime = 4f;
        [SerializeField] PatrolArea patrolArea = null;

        //Refs
        AIMovement movement;
        Damageable damageable;
        AICombat combat;
        GameObject player;
        ActionScheduler actionScheduler;

        //State
        float timeSinceArrivedAtRandomPoint = Mathf.Infinity;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        private void Awake()
        {
            movement = GetComponent<AIMovement>();
            damageable = GetComponent<Damageable>();
            combat = GetComponent<AICombat>();
            actionScheduler = GetComponent<ActionScheduler>();

            player = GameObject.FindWithTag("Player");
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (damageable.GetIsDead()) { return; }
            if (CanSeeTarget() && combat.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if(timeSinceLastSawPlayer <  suspicionTime)
            {
                actionScheduler.CancelCurrentAction();
            }
            else
            {
                PatrolAreaBehaviour();
            }
            UpdateTimers();
        }


        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            combat.Attack(player);
        }


        private bool CanSeeTarget()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }

        private void PatrolAreaBehaviour()
        {
            if (patrolArea == null) { return; }

            Vector3 nextPos = transform.position;
            if (AtRandomPoint())
            {
                timeSinceArrivedAtRandomPoint = 0;
                GetNewPoint();
            }

            nextPos = GetCurrentLocation();
            if (timeSinceArrivedAtRandomPoint > randomPointDwellTime)
            {
                movement.StartMoveAction(nextPos, 1);
            }
        }

        private Vector3 GetCurrentLocation()
        {
            return patrolArea.GetGeneratedPoint();
        }

        private void GetNewPoint()
        {
            patrolArea.GenerateRandomPoint();
        }

        private bool AtRandomPoint()
        {
            float distanceToPoint = Vector3.Distance(transform.position, GetCurrentLocation());
            return distanceToPoint < 1;
        }

        private void UpdateTimers()
        {
            timeSinceArrivedAtRandomPoint += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
#endif
    }
}
