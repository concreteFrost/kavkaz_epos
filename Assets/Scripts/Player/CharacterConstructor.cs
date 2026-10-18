using UnityEngine;

public class CharacterConstructor : MonoBehaviour
{

    [SerializeField] bool hasAllConsumables;
    [SerializeField] bool hasAllSpells;
    [SerializeField] bool hasAllWeapons;
    [Tooltip("Добавить все щиты из Resources в инвентарь на старте игры.")]
    [SerializeField] bool hasAllShields;

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

        if (hasAllShields)
        {
            weaponInv.AddAllShieldsOnStart();
        }


    }
}
