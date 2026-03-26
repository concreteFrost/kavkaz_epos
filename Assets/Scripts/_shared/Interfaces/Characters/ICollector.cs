public interface ICollector
{
    string CollectorId();
    CharacterStatsController StatsController { get; set; }
    IWeaponSetter CombatInventory { get; set; }
    IAttackSource AttackSource { get; set; }
    IDamagable Damagable { get; set; }
    IInteractable PickableItem { get; set; }
    void StartInteracion();
    void FinishInteraction();

    void DistributeItemToInventory(ItemData data);
}
