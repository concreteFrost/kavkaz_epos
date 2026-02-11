using UnityEngine;
using Zenject;

public class HumanoidAnimatorService
{
    public Animator animator;
    public AnimatorOverrideController overrideController;
    public IHumanoidMovement motor;
    public IHumanoidCombat combatController;
    public IDamagable damageController;
    public ITargetLocker targetLock;
    public IPushable pushReceiver;


    public HumanoidAnimatorService(
        Animator animator,
        AnimatorOverrideController overrideController,
        IHumanoidMovement motor,
        IHumanoidCombat combatController,
        ITargetLocker targetLock,
        IDamagable damageController,
        IPushable pushReceiver
        )
    {
        this.animator = animator;
        this.overrideController = overrideController;
        this.motor = motor;
        this.combatController = combatController;
        this.targetLock = targetLock;
        this.damageController = damageController;
        this.pushReceiver = pushReceiver;
    }
}
