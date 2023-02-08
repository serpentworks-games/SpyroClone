using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartNewGame()
    {
        StartCoroutine(LoadGame());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator LoadGame()
    {
        DontDestroyOnLoad(gameObject);

        ScreenFader fader = FindObjectOfType<ScreenFader>();

        yield return fader.FadeOut();
        yield return SceneManager.LoadSceneAsync(1);

        yield return fader.FadeWait();

        yield return fader.FadeIn();

        Destroy(gameObject);
    }
}
