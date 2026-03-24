using System;
using UnityEngine;

public class PlayerItemsCollector : BaseItemCollector
{

    CharacterSpellInventory spellInventory;
    PlayerConsumableInventory consumableInventory;

    public static Action<ItemData> LootCollected;
    public void Init(Transform self,
        CharacterStatsController statsController,
        BaseHumanoidAnimatorController animatorController,
        ICombatInventory combatInventory,
        IDamagable damageController,
        IAttackSource attackSource,
        CharacterSpellInventory spellInventory,
        PlayerConsumableInventory consumableInventory)
    {
        BaseInit(self, statsController, animatorController, combatInventory, damageController, attackSource);
        this.spellInventory = spellInventory;
        this.consumableInventory = consumableInventory; 
    }

    public override void DistributeItemToInventory(ItemData data)
    {
        
        if(data.itemSO is SpellProjectileSO) spellInventory.AddItemToInventory(data);
        if(data.itemSO is ConsumableItemSO) consumableInventory.AddItemToInventory(data);

        LootCollected?.Invoke(data);    
    }
}
