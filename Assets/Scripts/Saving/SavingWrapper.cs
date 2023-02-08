using System.Collections;
using System.Collections.Generic;
using SpyroClone.SceneManagement;
using UnityEngine;

namespace SpyroClone.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        SavingSystem savingSystem;
        ScreenFader screenFader;

        const string kSaveName = "save";

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
            screenFader = FindObjectOfType<ScreenFader>();
        }

        IEnumerator Start()
        {
            screenFader.FadeOutImmediate();
            yield return savingSystem.LoadLastScene(kSaveName);
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
            savingSystem.Save(kSaveName);
        }

        public void Load()
        {
            savingSystem.Load(kSaveName);
        }

        public void Delete()
        {
            savingSystem.Delete(kSaveName);
        }
    }
}
