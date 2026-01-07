using UnityEngine;

public class CharacterInteract : MonoBehaviour, ICollector 
{

    IAttackSource attackSource;

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
