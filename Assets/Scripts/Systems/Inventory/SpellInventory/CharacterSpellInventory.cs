
public class CharacterSpellInventory : QuickAccessInventory
{
    public void Init()
    {
        BaseInit();
    }
    public override void UseItem()
    {
        if (CurrentItem == null) return;

        var item = CurrentItem;
        item.quantity--;

        if (item.quantity <= 0)
        {

            RemoveFromInventory(item);
            return;
        }

        Notify(); //уведомляет
    }
}