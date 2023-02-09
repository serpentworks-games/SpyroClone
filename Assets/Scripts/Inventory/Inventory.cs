using SpyroClone.Saving;
using UnityEngine;

namespace SpyroClone.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        int gemCount;
        int basicKeysCount;
        int bossKeys;

        public int GetGemCount()
        {
            return gemCount;
        }

        public void SetGemCount(int amount)
        {
            gemCount = amount;
            if (gemCount < 0)
            {
                gemCount = 0;
            }
        }

        public void AddGems(int amount)
        {
            gemCount += amount;
        }

        public void RemoveGems(int amount)
        {
            gemCount -= amount;
        }

        public int GetBasicKeysCount()
        {
            return basicKeysCount;
        }

        public void SetBasicKeysCount(int amount)
        {
            basicKeysCount = amount;
            if (basicKeysCount < 0)
            {
                basicKeysCount = 0;
            }
        }

        public void AddBasicKey(int amount)
        {
            basicKeysCount += amount;
        }

        public void RemoveBasicKey(int amount)
        {
            basicKeysCount -= amount;
        }

        public int GetBossKeys()
        {
            return bossKeys;
        }

        public void SetBossKeys(int amount)
        {
            bossKeys = amount;
            if (bossKeys < 0)
            {
                bossKeys = 0;
            }
        }

        public void AddBossKey(int amount)
        {
            bossKeys += amount;
        }

        public void RemoveBossKey(int amount)
        {
            bossKeys -= amount;
        }

        //Saving System
        [System.Serializable]
        struct SaveData
        {
            public int gemCount;
            public int basicKeysCount;
            public int bossKeys;
        }

        public object CaptureState()
        {
            SaveData data = new();
            data.gemCount = gemCount;
            data.basicKeysCount = basicKeysCount;
            data.bossKeys = bossKeys;
            return data;
        }

        public void RestoreState(object state)
        {
            SaveData data = (SaveData)state;
            gemCount = data.gemCount;
            basicKeysCount = data.basicKeysCount;
            bossKeys = data.bossKeys;
        }
    }
}
