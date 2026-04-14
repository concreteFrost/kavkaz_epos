using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInteractionController : BaseCharacterInteractor
{

    CharacterSpellInventory spellInventory;
    PlayerConsumableInventory consumableInventory;
    CharacterWeaponInventory weaponInventory;
    PlayerQuestItemsInventory questItemsInventory;

    public static Action<ItemData> LootCollected;
    public void Init(string collectorId, Transform self,
        CharacterStatsController statsController,
        CharacterStatsModifier statsModifier,
        BaseHumanoidAnimatorController animatorController,
        IWeaponSetter combatInventory,
        IDamagable damageController,
        IAttackSource attackSource,
        ICharacterLifeCycle lifeCycle,
        CharacterSpellInventory spellInventory,
        PlayerConsumableInventory consumableInventory,
        CharacterWeaponInventory weaponInventory,
        PlayerQuestItemsInventory questItemsInventory)
    {
        BaseInit(collectorId, self, statsController, statsModifier, animatorController, combatInventory, damageController, attackSource, lifeCycle);
        this.spellInventory = spellInventory;
        this.consumableInventory = consumableInventory;
        this.weaponInventory = weaponInventory;
        this.questItemsInventory = questItemsInventory; 
    }

    private void OnEnable()
    {
        DialogueController.GrandRewards += OnRewardsGranted;
    }

    private void OnDisable()
    {
        DialogueController.GrandRewards -= OnRewardsGranted;
    }

    public override void DistributeItemToInventory(ItemData data)
    {

        if (data.itemSO is SpellProjectileSO) spellInventory.AddItemToInventory(data);
        if (data.itemSO is ConsumableItemSO) consumableInventory.AddItemToInventory(data);
        if (data.itemSO is CombatItemSO) weaponInventory.AddCombatItemToInventory(data);
        if (data.itemSO is QuestItemSO) questItemsInventory.AddItemToInventory(data);

        LootCollected?.Invoke(data);
    }

    public void OnRewardsGranted(List<ItemData> rewards)
    {
        foreach (ItemData item in rewards)
        {
            DistributeItemToInventory(item);
        }
    }
}
