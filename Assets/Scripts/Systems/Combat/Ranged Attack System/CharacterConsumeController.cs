using System;
using UnityEngine;

public class CharacterConsumeController : MonoBehaviour
{
    public bool isConsuming = false;
    BaseHumanoidAnimatorController animatorController;
    PlayerConsumableInventory inventory;
    public ItemData currentItem;
    public void Init(BaseHumanoidAnimatorController animatorController, PlayerConsumableInventory inventory)
    {
        this.animatorController = animatorController;
        this.inventory = inventory; 
    }


    public void StartConsume()
    {
        if (inventory.CurrentItem == null) return;

        currentItem = inventory.CurrentItem;

        var consumable = (ConsumableItemSO)currentItem.itemSO;

        if (consumable.consumableAnimation == null)
        {
            Consume();
            return;
        }

        animatorController.OverrideConsume(consumable.consumableAnimation);
        isConsuming = true;
    }

    public void StartConsumeFromContext(ItemData data)
    {
        currentItem = data;

        var consumable = (ConsumableItemSO)currentItem.itemSO;

        if ( consumable.consumableAnimation == null)
        {
            Consume();
            return;
        }

        animatorController.OverrideConsume(consumable.consumableAnimation);
        isConsuming = true;
    }


    public void Consume()
    {
        inventory.UseItem(currentItem);
    }

    internal void EndConsume()
    {
        isConsuming = false;
        currentItem = null;
    }

  
}