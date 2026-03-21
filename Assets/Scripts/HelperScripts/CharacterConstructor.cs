using UnityEngine;

public class CharacterConstructor : MonoBehaviour
{
    PlayerConsumableInventory consumableInventory;
    [SerializeField] bool hasAllConsumables;

    public void Init(PlayerConsumableInventory consumableInventory)
    {
        this.consumableInventory = consumableInventory;

        if (hasAllConsumables)
        {
            consumableInventory.AddAllItemsOnStart();
        }

    }
}
