using UnityEngine;

public class PlayerQuickSlotActionHandler : MonoBehaviour
{
    [SerializeField] CharacterSpellInventory spellInventory;
    PlayerActionGuards actionGuards;    
    public void Init(CharacterSpellInventory spellInventory, PlayerActionGuards actionGuards)
    {
        this.spellInventory = spellInventory;
        this.actionGuards = actionGuards;   
    }
    
    public void ChangeSpell(int dir)
    {
        if (!actionGuards.CanSwapSpell()) return;

        spellInventory.ChangeSpell(dir);
    }
}
