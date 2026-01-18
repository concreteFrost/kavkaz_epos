using UnityEngine;

public class CharacterInteract : MonoBehaviour, ICollector 
{

    public IAttackSource AttackSource { get; set; } =null;
    public IDamagable Damagable { get; set; } = null;
    /// <summary>
    /// Предмет для поднятия
    /// </summary>
    private IPickable pickable { get; set; }=null;

    /// <summary>
    /// Проверяется в OnTriggerEnter 
    /// </summary>
    public IPickable PickableItem { get => pickable; set => pickable = value; }

    public void Init(HumanoidInteractService service)
    { 
        AttackSource = service.combatInventory;
        Damagable = service.owner;
    }
    public void Interact()
    {

        if (pickable != null)
        {
            pickable.PickUp(this);  
        }
    }

  
}
