using SpyroClone.Inventories;
using UnityEngine;

namespace SpyroClone.Interactables
{
    public class LockedDoor : Interactable
    {
        [SerializeField] int numberOfKeysNeededToOpen = 0;
        [SerializeField] int numberOfBossKeysNeeded = 0;

        int currentKeys;
        int currentBossKeys;

        public override void CheckConditions(Inventory inventory)
        {
            if (numberOfBossKeysNeeded == 0 && numberOfKeysNeededToOpen == 0)
            {
                InteractWithObject(InteractRequirement.None);
                SetToTriggered();
                return;
            }
            else
            {
                if (numberOfKeysNeededToOpen != 0)
                {
                    currentKeys += inventory.GetBasicKeysCount();
                    inventory.RemoveBasicKey(currentKeys);

                    if (currentKeys <= numberOfKeysNeededToOpen)
                    {
                        InteractWithObject(InteractRequirement.BasicKey);
                        SetToTriggered();
                        return;
                    }
                }
                else if (numberOfBossKeysNeeded != 0)
                {
                    currentBossKeys += inventory.GetBossKeys();
                    inventory.RemoveBossKey(currentBossKeys);

                    if (currentBossKeys <= numberOfBossKeysNeeded)
                    {
                        InteractWithObject(InteractRequirement.BossKey);
                        SetToTriggered();
                        return;
                    }
                }
            }
        }
    }
}