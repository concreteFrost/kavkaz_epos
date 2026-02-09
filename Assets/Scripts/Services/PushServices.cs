using UnityEngine;

public class HumanoidPushServices
{
    public Transform self;

    public HumanoidAIMotor motor;
    public BaseHumanoidAnimatorController animatorController;

    public IDamagable damageController;
    public IRagdollController ragdollController;

    public HumanoidPushServices(Transform self, HumanoidAIMotor motor, BaseHumanoidAnimatorController animatorContoller, IDamagable damageController, IRagdollController ragdollController)
    {
        this.self = self;
        this.motor = motor;
        this.animatorController = animatorContoller;
        this.damageController = damageController;
        this.ragdollController = ragdollController;
       
    }
}

public class AgressivePushControllerServices
{
    public IAttackSource attackSource;
    public IHumanoidCombat combatController;
    public BaseHumanoidAnimatorController animatorController;
    public Transform self;

    public AgressivePushControllerServices(IAttackSource attackSource, IHumanoidCombat combatController, BaseHumanoidAnimatorController animatorController, Transform self)
    {
        this.attackSource = attackSource;
        this.combatController = combatController;
        this.animatorController = animatorController;
        this.self = self;
    }
}