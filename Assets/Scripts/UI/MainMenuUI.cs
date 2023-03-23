using SpyroClone.Saving;
using SpyroClone.SceneManagement;
using SpyroClone.Utils;
using UnityEngine;

namespace SpyroClone.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject continueButton;
        LazyValue<ScreenFader> fader;
        LazyValue<SavingWrapper> savingWrapper;

        private void Awake()
        {
            InitRefs();
        }

        private void Update()
        {
            if (!PlayerPrefs.HasKey("currentSaveFile"))
            {
                continueButton.SetActive(false);
            }
            continueButton.SetActive(true);
        }

        public void StartNewGame(string saveFile)
        {
            savingWrapper.Value.NewGame(saveFile);
        }

        public void EnableFileOptions(GameObject optionsToEnable)
        {
            optionsToEnable.gameObject.SetActive(true);
        }

        public void ContinueGame()
        {
            savingWrapper.Value.ContinueGame();
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void InitRefs()
        {
            fader = new LazyValue<ScreenFader>(GetScreenFader);
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        private ScreenFader GetScreenFader()
        {
            return FindObjectOfType<ScreenFader>();
        }

    }
}