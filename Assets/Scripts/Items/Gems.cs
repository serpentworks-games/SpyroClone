using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpyroClone.Items
{
    public class Gems : MonoBehaviour
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
    }
}
