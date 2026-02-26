using System.Linq;
using UnityEngine;

public class CharacterSpellInventory : QuickAccessInventory
{
    public void AddSpell(ItemData spell)
    {
        if (spell == null) return;

        items.Add(spell);

        Notify();
    }

    public void TopUpCurrentSpell(int quantity)
    {
        if (CurrentItem != null)
            CurrentItem.quantity += quantity;
    }

    public void UseSpell()
    {
        if (CurrentItem == null) return;

        CurrentItem.quantity--;

        if (CurrentItem.quantity <= 0)
        {
            
            RemoveAt(currentIndex);
            TryToRemoveFromQuickAccess(CurrentItem.itemSO);

        }


        Notify();
    }


    private void Start()
    {
        AddToQuickAccess(items[0]);
        
        //AddToQuickAccess(items[1]);
    }

    private void Update()
    {
        TestQuickSlot();
    }

   
}