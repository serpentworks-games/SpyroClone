using System;
using System.Collections;
using UnityEngine;

namespace SpyroClone.SceneManagement
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut()
        {
            Debug.Log("Fading out!");
            yield return StartCoroutine(Fade(1f, fadeOutTime));
        }

        public IEnumerator FadeWait()
        {
            yield return new WaitForSeconds(fadeWaitTime);
        }

        public IEnumerator FadeIn()
        {
            yield return StartCoroutine(Fade(0f, fadeInTime));
        }

        IEnumerator Fade(float targetAlpha, float fadeDuration)
        {
            float fadeSpeed = Mathf.Abs(canvasGroup.alpha - targetAlpha) / fadeDuration;

            while(!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.unscaledDeltaTime);
                yield return null;
            }

            yield return null;
        }
    }
}