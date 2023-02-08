using System.Collections;
using System.Collections.Generic;
using SpyroClone.Saving;
using UnityEngine;

namespace SpyroClone.Items
{
    public class Gems : MonoBehaviour, ISaveable
    {
        [SerializeField] int startingAmount = 0;

        int currentGemCount;

        public int GetGemCount()
        {
            return currentGemCount;
        }

        public void AddGems(int amountToAdd)
        {
            currentGemCount += amountToAdd;
        }

        public void RemoveGems(int amountToRemove)
        {
            currentGemCount -= amountToRemove;
            if (currentGemCount < 0)
            {
                currentGemCount = 0;
            }
        }

        public object CaptureState()
        {
            return currentGemCount;
        }

        public void RestoreState(object state)
        {
            int gemCount = (int)state;
            currentGemCount = gemCount;
        }
    }
}
