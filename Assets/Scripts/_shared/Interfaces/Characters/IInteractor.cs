
using UnityEngine;
public interface IInteractor
{
    string CollectorId();
    CharacterStatsController StatsController { get; set; }
    CharacterStatsModifier StatsModifier { get; set; }
    ICharacterLifeCycle LifeCycleController { get; set; }
    IWeaponSetter WeaponSetter { get; set; }
    IAttackSource AttackSource { get; set; }
    IDamagable Damagable { get; set; }
    IInteractable InteractableItem { get; set; }
    void StartInteracion();
    void FinishInteraction();
    void DistributeItemToInventory(ItemData data);

    Vector3 InteractorPosition();
}

public interface IPlayerInteractor : IInteractor
{
    CharacterWeaponInventory WeaponInventory { get; }
    PlayerConsumableInventory ConsumableInventory { get; }
    PlayerMoneyManager MoneyManager { get; }
}
