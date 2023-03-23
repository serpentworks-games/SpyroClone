using UnityEngine;

namespace SpyroClone.UI
{
    public class UIPanelSwitcher : MonoBehaviour
    {
        [SerializeField] GameObject menuEntryPoint;

        private void Start() {
            SwitchPanelTo(menuEntryPoint);
        }
        public void SwitchPanelTo(GameObject objectToShow)
        {
            if (objectToShow.transform.parent != transform) { return; }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(child.gameObject == objectToShow);
            }
        }
    }
}