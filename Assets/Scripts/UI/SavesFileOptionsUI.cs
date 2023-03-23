using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.Saving;
using SpyroClone.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpyroClone.UI
{
    public class SavesFileOptionsUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleTextBox;

        List<GameObject> saveFileButtons = new();

        LazyValue<SavingWrapper> savingWrapper;
        string fileNameToUse;

        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private void Start() {
            foreach (Transform child in transform)
            {
                saveFileButtons.Add(child.gameObject);
            }
        }

        public void SetupUI(string titleToUse, string fileNameToUse)
        {
            titleTextBox.text = titleToUse;
            this.fileNameToUse = fileNameToUse;
        }

        public void SetupTitle(string titleToUse)
        {
            titleTextBox.text = titleToUse;
        }
        public void SetupSaveFileName(string fileNameToUse)
        {
            this.fileNameToUse = fileNameToUse;
        }

        public void CreateNewGame()
        {
            savingWrapper.Value.NewGame(fileNameToUse);
        }

        public void LoadGame()
        {
            savingWrapper.Value.LoadSaveFile(fileNameToUse);
        }

        public void Delete()
        {
            savingWrapper.Value.DeleteSaveFile(fileNameToUse);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }
    }
}