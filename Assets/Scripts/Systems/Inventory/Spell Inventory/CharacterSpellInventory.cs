using UnityEngine;

public class CharacterSpellInventory : BaseInventory<SpellData>
{
    public void Init()
    {
        BaseInit();
        
    }

    private void Start()
    {
        Notify();
    }
    public override void UseItem(SpellData data)
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

    /// <summary>
    /// Выбирает для ИИ случайное заклинание с положительным запасом из всего инвентаря.
    /// </summary>
    public bool TrySelectRandomSpell()
    {
        var spells = items.FindAll(x =>
            x != null &&
            x.itemSO != null &&
            x.quantity > 0 &&
            x.usageChance > 0f);

        if (spells.Count == 0)
            return false;

        float totalChance = 0f;

        foreach (var spell in spells)
            totalChance += spell.usageChance;

        float choice = Random.Range(0f, totalChance);

        foreach (var spell in spells)
        {
            choice -= spell.usageChance;

            if (choice > 0f)
                continue;

            quickSlots[currentIndex] = spell;
            Notify();
            return true;
        }

        return false;
    }

    public void AddAllItemsOnStart()
    {
        var allItems = Resources.LoadAll<SpellProjectileSO>("Items/Spells/");

        foreach (var item in allItems)
        {
            var data = new SpellData
            {
                itemSO = item,
                quantity = 20
            };

            AddItemToInventory(data);
            AddToQuickAccess(data);

        }
    }

}