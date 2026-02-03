
using UnityEngine;

public interface ICollector 
{
    IDamagable Damagable { get; set; }
    ICombatInventory CombatInventory { get; set; }

    IAttackSource AttackSource { get; set; }
    public IPickable PickableItem { get; set; }
    void StartInteracion();
    void FinishInteraction();
}
