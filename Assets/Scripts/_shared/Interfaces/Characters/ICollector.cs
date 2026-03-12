public interface ICollector
{
    CharacterStatsController StatsController { get; set; }
    ICombatInventory CombatInventory { get; set; }
    IAttackSource AttackSource { get; set; }
    IDamagable Damagable { get; set; }
    IPickable PickableItem { get; set; }
    void StartInteracion();
    void FinishInteraction();
}
