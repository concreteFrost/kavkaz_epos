using UnityEngine;

public class HumanoidAnimatorService
{
    public Animator animator;
    public AnimatorOverrideController overrideController;
    public IHumanoidMovement motor;
    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public ITargetLocker targetLock;

    public HumanoidAnimatorService(
        Animator animator,
        AnimatorOverrideController overrideController,
        IHumanoidMovement motor,
        IHumanoidCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController)
    {
        this.animator = animator;
        this.overrideController = overrideController;
        this.motor = motor;
        this.combatController = combatController;
        this.targetLock = targetLock;
        this.damageController = damageController;
    }
}
