using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInteractionController : BaseCharacterInteractor
{
    private bool canLookForInteraction = true;

    CharacterSpellInventory spellInventory;
    PlayerConsumableInventory consumableInventory;
    CharacterWeaponInventory weaponInventory;
    PlayerQuestItemsInventory questItemsInventory;
    PlayerMoneyManager moneyManager;

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
        PlayerQuestItemsInventory questItemsInventory,
        PlayerMoneyManager moneyManager
        )
    {
        BaseInit(collectorId, self, statsController, statsModifier, animatorController, combatInventory, damageController, attackSource, lifeCycle);
        this.spellInventory = spellInventory;
        this.consumableInventory = consumableInventory;
        this.weaponInventory = weaponInventory;
        this.questItemsInventory = questItemsInventory;
        this.moneyManager = moneyManager;

       
    }

    private void OnEnable()
    {
        DialogueController.GrandRewards += OnRewardsGranted;
        GameStateManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        DialogueController.GrandRewards -= OnRewardsGranted;
        GameStateManager.GameStateChanged -= OnGameStateChanged;
    }

    public override void DistributeItemToInventory(ItemData data)
    {

        if (data.itemSO is SpellProjectileSO) spellInventory.AddItemToInventory(data);
        if (data.itemSO is ConsumableItemSO) consumableInventory.AddItemToInventory(data);
        if (data.itemSO is CombatItemSO) weaponInventory.AddCombatItemToInventory(data);
        if (data.itemSO is QuestItemSO) questItemsInventory.AddItemToInventory(data);
        if (data.itemSO is MoneyItemSO) moneyManager.AddMoney(data.quantity);

        LootCollected?.Invoke(data);
    }

    public void OnRewardsGranted(List<ItemData> rewards)
    {
        foreach (ItemData item in rewards)
        {
            DistributeItemToInventory(item);
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state != GameState.Game)
        {
            canLookForInteraction = false;
            interactable = null;
            InteractionLost?.Invoke();
            return;
        }

       
        canLookForInteraction = true;
    }

    protected override void HandleUpdateInteraction()
    {
        if (!canLookForInteraction)
        {
            return;
        }

        UpdateDetection();
    }
}
