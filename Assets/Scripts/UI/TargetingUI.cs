using System.Collections;
using System.Collections.Generic;
using SpyroClone.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace SpyroClone.UI
{
    public class TargetingUI : MonoBehaviour
    {
        TargetTracker tracker;
        Image targetImage;

        private void Awake()
        {
            tracker = FindObjectOfType<TargetTracker>();
            targetImage = GetComponentInChildren<Image>();
        }

        void Update()
        {
            if (tracker.GetActiveTarget() != null && tracker.GetIsLockedOn())
            {
                if (targetImage.enabled == false)
                {
                    targetImage.enabled = true;
                }

                transform.position = Camera.main.WorldToScreenPoint(tracker.GetActiveTarget().bounds.center);
            }
            else if (targetImage.enabled == true)
            {
                targetImage.enabled = false;
            }
        }
    }
}