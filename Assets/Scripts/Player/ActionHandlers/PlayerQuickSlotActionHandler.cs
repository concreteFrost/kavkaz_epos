using UnityEngine;

public class PlayerQuickSlotActionHandler : MonoBehaviour
{
    [SerializeField] CharacterSpellInventory spellInventory;
    [SerializeField] PlayerConsumableInventory consumableInventory;
    PlayerActionGuards actionGuards;    
    public void Init(CharacterSpellInventory spellInventory, PlayerConsumableInventory consumableInventory, PlayerActionGuards actionGuards)
    {
        this.spellInventory = spellInventory;
        this.actionGuards = actionGuards;
        this.consumableInventory = consumableInventory; 
    }
    
    public void ChangeSpell(int dir)
    {
        if (!actionGuards.CanSwapSpell()) return;

        spellInventory.Change(dir);
    }

    public void ChangeConsumable(int dir)
    {
        if(!actionGuards.CanSwapConsumables()) return;

        consumableInventory.Change(dir);
    }
}
