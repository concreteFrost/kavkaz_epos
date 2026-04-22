using System;
using UnityEngine;


public abstract class BaseCharacterInteractor : MonoBehaviour, IInteractor
{
    private Transform self;
    private BaseHumanoidAnimatorController animatorController;



    private string collectorId;
    public string CollectorId() => collectorId;
    public CharacterStatsController StatsController { get; set; } = null;
    public CharacterStatsModifier StatsModifier { get; set; } = null;
    public ICharacterLifeCycle LifeCycleController { get; set; } = null;
    public IWeaponSetter CombatInventory { get; set; } = null;
    public IDamagable Damagable { get; set; } = null;
    public IAttackSource AttackSource { get; set; } = null;

    protected IInteractable interactable = null;
    public IInteractable InteractableItem
    {
        get => interactable;
        set => interactable = value;
    }


    public bool CanPreventWeaponDamage() => Damagable.CharacterType != CharacterType.Player; // оружие ломается только у игрока

    private float interactRadius = 1f;

    public Action<IInteractable> InteractionAvailable;
    public Action InteractionLost;

    protected void BaseInit(
        string uniqueId,
        Transform self,
        CharacterStatsController statsController,
        CharacterStatsModifier statsModifier,
        BaseHumanoidAnimatorController animatorController,
        IWeaponSetter combatInventory,
        IDamagable damageController,
        IAttackSource attackSource,
        ICharacterLifeCycle lifeCycleController
        )
    {
        this.collectorId = uniqueId;
        this.self = self;
        this.StatsController = statsController;
        this.StatsModifier = statsModifier;
        this.animatorController = animatorController;
        this.CombatInventory = combatInventory;
        this.AttackSource = attackSource;
        this.Damagable = damageController;
        this.LifeCycleController = lifeCycleController;

        interactRadius = 1f;

    }

    private void Update()
    {
        HandleUpdateInteraction();
    }

    protected virtual void HandleUpdateInteraction()
    {
        UpdateDetection();
    }



    public void UpdateDetection()
    {
        var candidate = UpdatePickable();

        if (candidate == interactable)
            return;

        interactable = candidate;


        if (interactable != null)
        {
            InteractionAvailable?.Invoke(interactable);

        }
        else
        {
            InteractionLost?.Invoke();
        }
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
        {
            InteractionLost?.Invoke();
            return;
        }

        //проверяем угол только для сундуков и дверей
        var type = candidate.InteractType();

        if (type == ItemInteractionType.Chest || type == ItemInteractionType.Door)
        {
            Transform targetTransform = ((MonoBehaviour)candidate).transform;

            if (!IsFacingTarget(targetTransform))
                return; //не смотрим — не даём взаимодействовать
        }

        InteractableItem = candidate;

        GetInteractionAnimation(type);
    }

    public void FinishInteraction()
    {
        interactable.Interact(this);
        interactable = null;

        InteractionLost?.Invoke();
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