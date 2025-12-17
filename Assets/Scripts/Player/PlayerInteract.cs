using UnityEngine;

public class PlayerInteract : MonoBehaviour, ICollector 
{
    PlayerMotor animator;
    PlayerCombatController combatController;
    PlayerCombatInventory attackSource;

    private IPickable pickable { get; set; }=null;
    public IPickable PickableItem { get => pickable; set => pickable = value; }

    public void Init(PlayerInteractServiceProvider service)
    { 
        animator = service.motor;  
        attackSource = service.combatInventory;
        combatController = service.combatController;
    }

    public void Interact()
    {
        // ѕредотвращаем взаимодействие во врем€ атаки
        if (combatController.isAttacking)
            return;

        if (pickable != null)
        {
            pickable.PickUp(attackSource);  
        }
    }

  
}
