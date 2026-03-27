using System;
using UnityEngine;

public class PlayerItemsCollector : BaseItemCollector
{

    CharacterSpellInventory spellInventory;
    PlayerConsumableInventory consumableInventory;
    CharacterWeaponInventory weaponInventory;

    public static Action<ItemData> LootCollected;
    public void Init(string collectorId, Transform self,
        CharacterStatsController statsController,
        BaseHumanoidAnimatorController animatorController,
        IWeaponSetter combatInventory,
        IDamagable damageController,
        IAttackSource attackSource,
        CharacterSpellInventory spellInventory,
        PlayerConsumableInventory consumableInventory,
        CharacterWeaponInventory weaponInventory)
    {
        BaseInit(collectorId, self, statsController, animatorController, combatInventory, damageController, attackSource);
        this.spellInventory = spellInventory;
        this.consumableInventory = consumableInventory; 
        this.weaponInventory = weaponInventory;
    }

    public override void DistributeItemToInventory(ItemData data)
    {
        
        if(data.itemSO is SpellProjectileSO) spellInventory.AddItemToInventory(data);
        if(data.itemSO is ConsumableItemSO) consumableInventory.AddItemToInventory(data);
        if(data.itemSO is CombatItemSO) weaponInventory.AddCombatItemToInventory(data);

        LootCollected?.Invoke(data);    
    }
}
