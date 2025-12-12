using UnityEngine;

public class PlayerInteract : MonoBehaviour, ICollector 
{
    PlayerMotor animator;
    PlayerCombatInventory attackSource;

    private IPickable pickable { get; set; }=null;
    public IPickable PickableItem { get => pickable; set => pickable = value; }

    public void Init(PlayerInteractServiceProvider service)
    { 
        animator = service.motor;  
        attackSource = service.combatInventory;
    }

    public void Interact()
    {
        // ѕредотвращаем взаимодействие во врем€ атаки
        if (animator.IsAttacking)
            return;

        if (pickable != null)
        {
            pickable.PickUp(attackSource);  
        }
    }

  
}
