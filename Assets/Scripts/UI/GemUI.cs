using SpyroClone.Inventories;
using TMPro;
using UnityEngine;
using SpyroClone.Inventories;

namespace SpyroClone.UI
{
    public class GemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI gemValue;

        Inventory inventory;

        private void Awake()
        {
            inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }

        private void Update()
        {
            gemValue.text = inventory.GetGemCount().ToString();
        }
    }
}
