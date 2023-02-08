using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyroClone.Core
{
    public class PersistantObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectsPrefab;

        static bool sHasSpawned = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            sHasSpawned = false;
        }

        private void Awake()
        {
            if (sHasSpawned) { return; }

            SpawnPersistantObjects();

            sHasSpawned = true;
        }

        private void SpawnPersistantObjects()
        {
            GameObject instance = Instantiate(persistantObjectsPrefab);
            DontDestroyOnLoad(instance);
        }
    }
}