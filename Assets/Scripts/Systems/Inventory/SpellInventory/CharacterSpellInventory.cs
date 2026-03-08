
public class CharacterSpellInventory : QuickAccessInventory
{
    public void Init()
    {
        BaseInit();
    }
    public override void UseItem(ItemData data)
    {
        if (data == null) return;

        var item = data;
        item.quantity--;

        if (item.quantity <= 0)
        {

            RemoveFromInventory(item);
            return;
        }

        Notify(); //уведомляет
    }
}