using System;
using UnityEngine;

public class PlayerInteract : MonoBehaviour, ICollector
{
    ICharacterAnimator animator;
    PlayerCombatInventory combatInventory;

    private IPickable pickable { get; set; }=null;
    public IPickable PickableItem { get => pickable; set => pickable = value; }

    public void Init(PlayerCombatInventory _combatInventory, ICharacterAnimator anim)
    {
        combatInventory = _combatInventory; 
        animator = anim;    
    }

    public void Interact()
    {
        if (animator.IsAttacking)
            return;

        if (pickable != null)
        {
            HandleItemPick(pickable.Type());
        }
    }

    private void HandleItemPick(PickableType type)
    {
        switch (type){
            case PickableType.weapon:
                PickWeapon();
                break;
            case PickableType.inventoryItem:
                PickInventoryItem();
                break;
            case PickableType.shield:
                PickShield();
                break;
            default:
                PickInventoryItem();
                break;
        }
    }

    private void PickInventoryItem()
    {
        pickable.PickUp(this.transform);
    }

    private void PickWeapon()
    {
        bool canOverride = combatInventory.currentWeapon.WeaponData().canOverride;

        if (canOverride)
        {
            pickable.PickUp(combatInventory.GetRightHand());
            combatInventory.SetWeapon(pickable as IWeapon);

        }
    }

    private void PickShield()
    {

        if (combatInventory.shieldWeapon == null)
        {
            pickable.PickUp(combatInventory.GetLeftHand());
        }
    }
}
