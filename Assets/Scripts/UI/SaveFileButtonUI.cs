using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpyroClone.UI
{
    public class SaveFileButtonUI : MonoBehaviour
    {
        [SerializeField] string saveFileTitle = "";
        [SerializeField] string saveFileName = "";
        [SerializeField] bool hasBeenFilled = false;

        [SerializeField] TMPro.TMP_Text saveFileNameText;

        public void SetHasBeenFilled()
        {
            hasBeenFilled = true;
        }

        public string GetFileName()
        {
            return saveFileName;
        }

        public string GetTitle()
        {
            return saveFileTitle;
        }

        public void SetFileNameText(string save)
        {
            saveFileNameText.text = save;
        }
    }
}