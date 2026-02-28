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

        var item = CurrentItem;
        item.quantity--;

        if (item.quantity <= 0)
        {
            RemoveFromInventory(item);
            return;
        }

        Notify();
    }

    //private void Update()
    //{
    //    TestQuickSlot();
    //}

                                                     

   
}