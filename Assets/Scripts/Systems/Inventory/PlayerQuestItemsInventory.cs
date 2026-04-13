using UnityEngine;

public class PlayerQuestItemsInventory : QuickAccessInventory
{
    public override void UseItem(ItemData data)
    {
       if(data == null) return; 

       RemoveFromInventory(data);

        Notify();
    }

    public void Init()
    {
        BaseInit();
    }
}
