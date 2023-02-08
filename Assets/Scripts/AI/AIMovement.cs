using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.Core;
using SpyroClone.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace SpyroClone.AI
{
    public class AIMovement : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxMoveSpeed = 5f;
        [SerializeField][Range(0, 1)] float speedFraction = 1;
        [SerializeField] float maxNavPathLength = 40f;

        NavMeshAgent agent;
        Animator animator;
        ActionScheduler actionScheduler;
        Damageable damageable;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            damageable = GetComponent<Damageable>();
        }
        // Update is called once per frame
        void Update()
        {
            agent.enabled = !damageable.GetIsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionScheduler.StartAction(this);
            MoveToDestination(destination, speedFraction);
        }

        public void MoveToDestination(Vector3 destination, float speedFraction)
        {
            agent.speed = maxMoveSpeed * Mathf.Clamp01(speedFraction);
            if (agent.enabled == false) { return; }
            agent.isStopped = false;
            agent.destination = destination;
        }

        public bool CanMoveToDest(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVel = transform.InverseTransformDirection(velocity);
            float speed = localVel.z;

            animator.SetFloat("forwardSpeed", speed);
        }

        public void Cancel()
        {
            if (agent.enabled == false) { return; }
            agent.isStopped = true;
        }

        //Saving

        [System.Serializable]
        struct SaveData
        {
            public SerializableVector3 position;
            public SerializableQuaternion rotation;
        }

        public object CaptureState()
        {
            SaveData data = new()
            {
                position = new SerializableVector3(transform.position),
                rotation = new SerializableQuaternion(transform.rotation)
            };
            return data;
        }

        public void RestoreState(object state)
        {
            SaveData data = (SaveData)state;
            transform.position = data.position.ToVector();
            transform.rotation = data.rotation.ToQuaternion();
        }
    }
}