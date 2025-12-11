using UnityEngine;

public class PlayerInteract : MonoBehaviour, ICollector 
{
    ICharacterAnimator animator;
    IAttackSource attackSource;

    private IPickable pickable { get; set; }=null;
    public IPickable PickableItem { get => pickable; set => pickable = value; }

    public void Init(PlayerInteractServiceProvider service)
    {
       
        animator = service.motor;  
        attackSource = service.combatInventory;
    }

    public void Interact()
    {
        if (animator.IsAttacking)
            return;

        if (pickable != null)
        {
            pickable.PickUp(attackSource);  
        }
    }

  
}
