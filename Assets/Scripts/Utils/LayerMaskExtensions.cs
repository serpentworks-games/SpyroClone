using UnityEngine;

namespace SpyroClone.Utils
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask layerMask, GameObject go)
        {
            return 0 != (layerMask.value & 1 << go.layer);
        }
    }
}