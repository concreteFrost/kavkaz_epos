using UnityEngine;

enum InteractionType
{
    FromFloor = 0,
    BodyLevel = 1
}
public abstract class BaseItemCollector : MonoBehaviour, ICollector
{
    private Transform self;
    private BaseHumanoidAnimatorController animatorController;

    public CharacterStatsController StatsController { get; set; }=null;
    public ICombatInventory CombatInventory { get; set; } = null;
    public IDamagable Damagable { get; set; } = null;
    public IAttackSource AttackSource { get; set; } = null;

    private IPickable pickable = null;
    public IPickable PickableItem
    {
        get => pickable;
        set => pickable = value;
    }


    public bool CanPreventWeaponDamage() => Damagable.CharacterType != CharacterType.Player; // оружие ломается только у игрока

    private float interactRadius=1f;

    protected void BaseInit(Transform self,
        CharacterStatsController statsController,
        BaseHumanoidAnimatorController animatorController,
        ICombatInventory combatInventory,
        IDamagable damageController,
        IAttackSource attackSource)
    {
        this.self = self;
        this.StatsController = statsController;
        this.animatorController = animatorController;
        this.CombatInventory = combatInventory;
        this.AttackSource = attackSource;
        this.Damagable = damageController;

        interactRadius = 1f;
    }


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



    public void StartInteracion()
    {

        if (UpdatePickable() == null)
            return;

        PickableItem = UpdatePickable();

        //PickableItem.PickUp(this);
        animatorController.PlayClipCrossFade(AnimatorParameters.interactMidLevelClip); 
        //PickableItem = null;
    }

    public void FinishInteraction()
    {
        pickable.PickUp(this);
        pickable = null;
    }

    public abstract void DistributeItemToInventory(ItemData data);
}