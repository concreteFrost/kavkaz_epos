
using UnityEngine;

public interface ICollector 
{
    IDamagable Damagable { get; set; }
    ICombatInventory AttackSource { get; set; }
    public IPickable PickableItem { get; set; }
    void Interact();
}
