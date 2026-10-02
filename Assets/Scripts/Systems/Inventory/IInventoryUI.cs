using System.Collections.Generic;

public interface IInventoryUI
{
    IEnumerable<ItemData> GetItemsForUI();
    IEnumerable<ItemData> GetQuickAccessDataForUI();
    void AddToQuickAccess(ItemData item);
    void RemoveFromQuickAccess(ItemData item);
    void RemoveFromInventory(ItemData item);
    void UseItem(ItemData item);
}
