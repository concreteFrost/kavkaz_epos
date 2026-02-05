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