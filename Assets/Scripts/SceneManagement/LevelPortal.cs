using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.Saving;
using SpyroClone.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpyroClone.SceneManagement
{
    public class LevelPortal : MonoBehaviour
    {
        [Header("Scene and Portal to Move To")]
        [SceneName][SerializeField] string sceneName;
        [SerializeField] PortalIdentifier portalIdentifier;

        [Header("Utils")]
        [SerializeField] Transform spawnPoint;

        enum PortalIdentifier
        {
            A, B, C, D, E, F,
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(SceneTransition());
            }
        }

        IEnumerator SceneTransition()
        {
            DontDestroyOnLoad(gameObject);

            ScreenFader fader = FindObjectOfType<ScreenFader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut();
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneName);
            savingWrapper.Load();

            LevelPortal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            
            yield return fader.FadeWait();

            savingWrapper.Save();

            yield return fader.FadeIn();
            Destroy(gameObject);
        }

        private LevelPortal GetOtherPortal()
        {
            LevelPortal[] foundPortals = FindObjectsOfType<LevelPortal>();
            foreach (var portal in foundPortals)
            {
                if (portal == this) { continue; }
                if (portal.portalIdentifier != portalIdentifier) { continue; }
                return portal;
            }
            return null;
        }

        private void UpdatePlayer(LevelPortal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }
    }
}