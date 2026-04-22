public interface IInteractor
{
    string CollectorId();
    CharacterStatsController StatsController { get; set; }
    CharacterStatsModifier StatsModifier { get; set; }
    ICharacterLifeCycle LifeCycleController { get; set; }
    IWeaponSetter CombatInventory { get; set; }
    IAttackSource AttackSource { get; set; }
    IDamagable Damagable { get; set; }
    IInteractable InteractableItem { get; set; }
    void StartInteracion();
    void FinishInteraction();
    void DistributeItemToInventory(ItemData data);
}
