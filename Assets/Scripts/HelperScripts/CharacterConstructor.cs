using UnityEngine;

public class CharacterConstructor : MonoBehaviour
{
    PlayerConsumableInventory consumableInventory;
    CharacterSpellInventory spellInventory;
    [SerializeField] bool hasAllConsumables;
    [SerializeField] bool hasAllSpells;

    public void Init(PlayerConsumableInventory consumableInventory, CharacterSpellInventory spellInventory)
    {
        this.consumableInventory = consumableInventory;
        this.spellInventory = spellInventory;

        if (hasAllConsumables)
        {
            consumableInventory.AddAllItemsOnStart();
        }

        if (hasAllSpells)
        {
            spellInventory.AddAllItemsOnStart();
        }

    }
}
