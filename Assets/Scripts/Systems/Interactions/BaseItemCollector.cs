using UnityEngine;


public abstract class BaseItemCollector : MonoBehaviour, ICollector
{
    private Transform self;
    private BaseHumanoidAnimatorController animatorController;

    private string collectorId;
    public string CollectorId() => collectorId;
    public CharacterStatsController StatsController { get; set; }=null;
    public ICombatInventory CombatInventory { get; set; } = null;
    public IDamagable Damagable { get; set; } = null;
    public IAttackSource AttackSource { get; set; } = null;

    private IInteractable pickable = null;
    public IInteractable PickableItem
    {
        get => pickable;
        set => pickable = value;
    }


    public bool CanPreventWeaponDamage() => Damagable.CharacterType != CharacterType.Player; // оружие ломается только у игрока

    private float interactRadius=1f;

    protected void BaseInit(string uniqueId, Transform self,
        CharacterStatsController statsController,
        BaseHumanoidAnimatorController animatorController,
        ICombatInventory combatInventory,
        IDamagable damageController,
        IAttackSource attackSource)
    {
        this.collectorId = uniqueId;
        this.self = self;
        this.StatsController = statsController;
        this.animatorController = animatorController;
        this.CombatInventory = combatInventory;
        this.AttackSource = attackSource;
        this.Damagable = damageController;

        interactRadius = 1f;
    }


    public IInteractable UpdatePickable()
    {
        Collider[] hits = Physics.OverlapSphere(
            self.position,
            interactRadius
        );

        float minDistance = float.MaxValue;
        IInteractable nearest = null;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent(out IInteractable candidate))
                continue;

            if (candidate.HasInteracted)
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

    protected void GetInteractionAnimation(ItemInteractionType type)
    {
        switch (type)
        {
            case ItemInteractionType.Item:
                animatorController.PlayClipCrossFade(AnimatorParameters.itemInteract);
                break;
            case ItemInteractionType.Chest:
                animatorController.PlayClipCrossFade(AnimatorParameters.chsetInteract);
                break;
            default:
                animatorController.PlayClipCrossFade(AnimatorParameters.itemInteract);
                break;
        }
    }

    public void StartInteracion()
    {
        var candidate = UpdatePickable();

        if (candidate == null)
            return;

        //проверяем угол только для сундуков и дверей
        var type = candidate.InteractType();

        if (type == ItemInteractionType.Chest || type == ItemInteractionType.Door)
        {
            Transform targetTransform = ((MonoBehaviour)candidate).transform;

            if (!IsFacingTarget(targetTransform))
                return; //не смотрим — не даём взаимодействовать
        }

        PickableItem = candidate;

        GetInteractionAnimation(type);
    }

    public void FinishInteraction()
    {
        pickable.Interact(this);
        pickable = null;
    }

    private bool IsFacingTarget(Transform target, float maxAngle = 60f)
    {
        Vector3 directionToTarget = (target.position - self.position).normalized;
        directionToTarget.y = 0f;

        Vector3 forward = self.forward;
        forward.y = 0f;

        float angle = Vector3.Angle(forward, directionToTarget);

        return angle <= maxAngle;
    }

    public abstract void DistributeItemToInventory(ItemData data);
}