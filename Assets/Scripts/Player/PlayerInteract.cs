using UnityEngine;

public class PlayerInteract : MonoBehaviour, ICollector 
{

    PlayerCombatInventory attackSource;

    /// <summary>
    /// Предмет для поднятия
    /// </summary>
    private IPickable pickable { get; set; }=null;

    /// <summary>
    /// Проверяется в OnTriggerEnter 
    /// </summary>
    public IPickable PickableItem { get => pickable; set => pickable = value; }

    public void Init(PlayerInteractServices service)
    { 
        attackSource = service.combatInventory;
    }
    public void Interact()
    {

        if (pickable != null)
        {
            pickable.PickUp(attackSource);  
        }
    }

  
}
