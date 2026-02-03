using UnityEngine;

enum InteractionType
{
    FromFloor = 0,
    BodyLevel = 1
}
public class CharacterInteract : MonoBehaviour, ICollector
{
    private Transform self;
    private BaseHumanoidAnimatorController animatorController;

    public ICombatInventory CombatInventory { get; set; } = null;
    public IDamagable Damagable { get; set; } = null;
    public IAttackSource AttackSource { get; set; }

    private IPickable pickable = null;
    public IPickable PickableItem
    {
        get => pickable;
        set => pickable = value;
    }

    private float interactRadius;


    public void Init(HumanoidInteractService service)
    {
        self = service.self;
        animatorController = service.animatorController;
        CombatInventory = service.combatInventory;
        AttackSource = service.attackSource;
        Damagable = service.owner;

        interactRadius = 1f;
       
    }

    /// <summary>
    /// Вызывается извне (input / service)
    /// </summary>
    public IPickable UpdatePickable()
    {
        Collider[] hits = Physics.OverlapSphere(
            self.position,
            interactRadius
        );

        float minDistance = float.MaxValue;
        IPickable nearest = null;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out IPickable candidate))
                continue;

            if (candidate.IsPicked)
                continue;

            float distance = Vector3.SqrMagnitude(
                hit.transform.position - self.position
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = candidate;
            }
        }

        return nearest;
    }

    public void StartInteracion()
    {
        
        if (UpdatePickable() == null)
            return;

        PickableItem = UpdatePickable();

        //PickableItem.PickUp(this);
        animatorController.PerformInteract();
        //PickableItem = null;
    }

    public void FinishInteraction()
    {
        pickable.PickUp(this);
        pickable = null;
    }
}
