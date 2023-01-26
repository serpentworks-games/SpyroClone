using SpyroClone.Items;
using TMPro;
using UnityEngine;

namespace SpyroClone.UI
{
    public class GemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI gemValue;

        Gems gems;

        private void Awake() {
            gems = GameObject.FindWithTag("Player").GetComponent<Gems>();
        }

        private void Update() {
            gemValue.text = gems.GetGemCount().ToString();
        }
    }
}
