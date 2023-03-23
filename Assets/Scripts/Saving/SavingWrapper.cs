using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpyroClone.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        SavingSystem savingSystem;
        ScreenFader screenFader;

        const int kFirstSceneBuildIndex = 1;
        const string kCurrentSaveKey = "currentSaveName";

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
            screenFader = FindObjectOfType<ScreenFader>();
        }

        public void NewGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(kCurrentSaveKey);
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(kCurrentSaveKey, saveFile);
        }

        public void ContinueGame()
        {
            StartCoroutine(LoadLastScene());
        }

        IEnumerator LoadFirstScene()
        {
            yield return screenFader.FadeOut();
            yield return SceneManager.LoadSceneAsync(kFirstSceneBuildIndex);
            yield return screenFader.FadeIn();
        }

        IEnumerator LoadLastScene()
        {
            yield return screenFader.FadeOut();
            yield return savingSystem.LoadLastScene(GetCurrentSave());
            yield return screenFader.FadeIn();

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.F3))
            {
                Delete();
            }

        }
        public void Save()
        {
            savingSystem.Save(GetCurrentSave());
        }

        public void Load()
        {
            screenFader.FadeOutImmediate();
            savingSystem.Load(GetCurrentSave());
            screenFader.FadeIn();
        }

        public void LoadSaveFile(string saveFile)
        {
            screenFader.FadeOutImmediate();
            savingSystem.Load(saveFile);
            screenFader.FadeIn();
        }

        public void Delete()
        {
            savingSystem.Delete(GetCurrentSave());
        }

        public void DeleteSaveFile(string saveFile)
        {
            savingSystem.Delete(saveFile);
        }

        public IEnumerable<string> ListSaves()
        {
            return savingSystem.ListSaves();
        }
    }
}
