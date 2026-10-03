using UnityEngine;

public class CharacterConstructor : MonoBehaviour
{

    [SerializeField] bool hasAllConsumables;
    [SerializeField] bool hasAllSpells;
    [SerializeField] bool hasAllWeapons;

    public void Init(PlayerConsumableInventory consumableInventory, CharacterSpellInventory spellInventory, CharacterWeaponInventory weaponInv
        )
    {

        if (hasAllConsumables)
        {
            consumableInventory.AddAllItemsOnStart();
        }

        if (hasAllSpells)
        {
            spellInventory.AddAllItemsOnStart();
        }

        if (hasAllWeapons)
        {
            weaponInv.AddAllItemsOnStart();
        }


    }
}
