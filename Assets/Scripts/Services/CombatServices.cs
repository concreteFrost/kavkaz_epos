using UnityEngine;

public class BaseHumanoidCombatControllerServices
{
    public ICombatInventory combatInventory;
    public BaseHumanoidAnimatorController animatorController;
    public IDamagable damageController;

    public BaseHumanoidCombatControllerServices(
        ICombatInventory combatInventory,
        BaseHumanoidAnimatorController animatorController,
        IDamagable damageController
       )
    {
        this.combatInventory = combatInventory;
        this.animatorController = animatorController;
        this.damageController = damageController;   
    }

}

public class HumanoidAICombatControllerServices : BaseHumanoidCombatControllerServices
{
    public IPushable pushable;

    public HumanoidAICombatControllerServices(
        ICombatInventory combatInventory,
        BaseHumanoidAnimatorController animatorController,
        IDamagable damageController,
        IPushable pushable  // новое поле
    ) : base(combatInventory, animatorController, damageController) // вызов конструктора базового класса
    {
        this.pushable = pushable; // инициализация нового поля
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

public class AgressivePushServices
{
    public IAttackSource attackSource;
    public IHumanoidCombat combatController;
    public BaseHumanoidAnimatorController animatorController;

    public AgressivePushServices(IAttackSource attackSource, IHumanoidCombat combatController, BaseHumanoidAnimatorController animatorController)
    {
        this.attackSource = attackSource;
        this.combatController = combatController;
        this.animatorController = animatorController;
    }
}

