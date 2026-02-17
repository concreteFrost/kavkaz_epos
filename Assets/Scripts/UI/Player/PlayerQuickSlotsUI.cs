using UnityEngine;

public class PlayerQuickSlotsUI : MonoBehaviour
{
    [SerializeField] QuickSlotBreakableUI weaponItem;
    [SerializeField] QuickSlotBreakableUI shieldItem;
    [SerializeField] QuickSlotItemUI spellItem;
    [SerializeField] QuickSlotItemUI resourceItem;
    
    CharacterSpellInventory spellInventory;
    HumanoidCombatInventory combatInventory;
    public void Init(CharacterSpellInventory spellInventory, HumanoidCombatInventory combatInventory)
    {
        this.spellInventory = spellInventory;
        this.combatInventory = combatInventory; 

        spellInventory.UpdateSpell += OnSpellUpdated;
        this.spellInventory.GetCurrentSpell();

        combatInventory.WeaponDataUpdated += OnWeaponUpdated;
        this.combatInventory.GetCurrentWeaponData();

        combatInventory.ShieldUpdated += OnShieldUpdated;
        this.combatInventory.GetCurrentShieldData();    
    }

    private void OnDisable()
    {
        spellInventory.UpdateSpell -= OnSpellUpdated;
        combatInventory.WeaponDataUpdated -= OnWeaponUpdated;  
        combatInventory.ShieldUpdated -= OnShieldUpdated;   
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

    private void OnWeaponUpdated(ItemSO data, IBreakable weapon)
    {
        if(weapon != combatInventory.DefaultWeapon)
        {
            weaponItem.UpdateWeaponData(data, weapon);
        }
        else
        {
            weaponItem.RemoveData();
        }
    }

    private void OnShieldUpdated(ItemSO data,IBreakable shield)
    {
        if(shield != null)
        {
            shieldItem.UpdateWeaponData(data,shield);
        }
        else
        {
            shieldItem.RemoveData();   
        }

    }
}
