public class PlayerKeyItemsInventory : QuickAccessInventory
{
    public override void UseItem(ItemData data)
    {
        if (data == null) return;

        RemoveFromInventory(data);

        Notify();
    }

    public void Init()
    {
        BaseInit();
    }

    public bool HasTargetKey(string id)
    {
        var targetKey = items.Find((x) => x.itemSO.id == id);

        if (targetKey == null) return false;

        UseItem(targetKey);

        return true;
    }
}
