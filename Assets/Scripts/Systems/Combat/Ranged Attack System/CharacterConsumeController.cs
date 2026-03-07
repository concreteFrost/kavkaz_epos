using System;
using UnityEngine;

public class CharacterConsumeController : MonoBehaviour
{
    public bool isConsuming = false;
    BaseHumanoidAnimatorController animatorController;
    CharacterConsumableInventory inventory;
    public void Init(BaseHumanoidAnimatorController animatorController, CharacterConsumableInventory inventory)
    {
        this.animatorController = animatorController;
        this.inventory = inventory; 
    }


    public void StartConsume()
    {
        if (inventory.CurrentItem == null) return;

        var contextItem = GetAnimtionClip();

        if(contextItem == null)
        {
            Consume();
            return;
        }

        animatorController.OverrideConsume(contextItem);
        isConsuming = true;
    }

    public void Consume()
    {
        inventory.UseItem();
    }

    internal void EndConsume()
    {
        isConsuming = false;
    }

    public AnimationInfoSO GetAnimtionClip()
    {
        if(inventory.CurrentItem ==null) return null;

        switch (inventory.CurrentItem.itemSO)
        {
            case WeaponModifierItemSO weaponItem:
                return weaponItem.consumableAnimation;          
            case InstantStatModifierItemSO statItem:
                return statItem.consumableAnimation;
            default:
                return null;
        }
    }
}