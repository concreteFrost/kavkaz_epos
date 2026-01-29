using UnityEngine;

public class HumanoidCombatControllerServices
{
    public ICombatInventory combatInventory;
    public BaseHumanoidAnimatorController animatorController;

    public HumanoidCombatControllerServices(
        ICombatInventory combatInventory,
        BaseHumanoidAnimatorController animatorController
       )
    {
        this.combatInventory = combatInventory;
        this.animatorController = animatorController;
    }

}

public class HumanoidCombatInventoryServices
{
    public BaseHumanoidAnimatorController animatorController;
    public IHumanoidCombat combatController;
    public ICollector collector;
    public IAttackSource initialAttackSource;
    //public ICharacterStatsController statsModifier;
    public Transform sourcePosition;
    public int sourceId;

    public HumanoidCombatInventoryServices(
       BaseHumanoidAnimatorController animatorController,
        IHumanoidCombat combatController,
        ICollector collector,
        IAttackSource initialAttackSource,
        //ICharacterStatsController stats,
        Transform sourcePosition,
        int sourceId)
    {
        this.animatorController = animatorController;
        this.combatController = combatController;
        this.collector = collector;
        this.initialAttackSource = initialAttackSource;
        this.sourcePosition = sourcePosition;
        //this.statsModifier = stats;
        this.sourceId = sourceId;
    }
}

public class AttackSourceServices
{
    public Transform sourcePosition;
    public int sourceId;

    public AttackSourceServices(Transform sourcePosition, int sourceId)
    {
        this.sourcePosition = sourcePosition;
        this.sourceId = sourceId;
    }
}

