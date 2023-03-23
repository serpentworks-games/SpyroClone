using SpyroClone.Saving;
using UnityEngine;
using UnityEngine.UI;

namespace SpyroClone.UI
{
    public class SaveListUI : MonoBehaviour
    {
        [SerializeField] SavesFileOptionsUI savesFileOptionsUI;
        [SerializeField] GameObject saveFileButtonPrefab;

        public void SetupOptions(SaveFileButtonUI saveFileSlot)
        {
            savesFileOptionsUI.SetupUI(saveFileSlot.GetTitle(), saveFileSlot.GetFileName());
        }

        private void OnEnable() {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            foreach (string save in savingWrapper.ListSaves())
            {
                GameObject btnInstance = Instantiate(saveFileButtonPrefab, transform);
                SaveFileButtonUI buttonUI = btnInstance.GetComponent<SaveFileButtonUI>();
                buttonUI.SetFileNameText(save);
            }
        }
    }
}