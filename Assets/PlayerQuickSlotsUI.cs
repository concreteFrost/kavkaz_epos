using UnityEngine;

public class PlayerQuickSlotsUI : MonoBehaviour
{
    [SerializeField] QuickSlotItemUI weaponItem;
    [SerializeField] QuickSlotItemUI shieldItem;
    [SerializeField] QuickSlotItemUI spellItem;
    [SerializeField] QuickSlotItemUI resourceItem;
    
    CharacterSpellInventory spellInventory;
    public void Init(CharacterSpellInventory spellInventory)
    {
        this.spellInventory = spellInventory;

        spellInventory.UpdateSpell += OnSpellUpdated;
        this.spellInventory.GetCurrentSpell();
    }

    private void OnDisable()
    {
        spellInventory.UpdateSpell -= OnSpellUpdated;
    }

    private void OnSpellUpdated(ItemData currentSpell)
    {
        if(spellInventory.CurrentSpell != null)
        {
            spellItem.UpdateImageDate(currentSpell);
        }
        else
        {
            Debug.Log("spell is null");
            spellItem.RemoveData();
        }
    }
}
