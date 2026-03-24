
using UnityEngine;

public class HumanoidAIDamageController : BaseHumanoidDamageController
{

    IRagdollController ragdollController;

    public void Init(
        Transform self,
        IHumanoidMovement motor,
        CharacterStatsController statsController,
        CharacterStatsModifier statsModifier,
        IRagdollController ragdollController,
        BaseHumanoidAnimatorController animatorController
        )
    {
        BaseInit(animatorController: animatorController, statsModifier: statsModifier, statsController: statsController, motor: motor, self: self);

        this.ragdollController = ragdollController;
        ragdollController.Recovered += OnRecover;

    }


    private void OnDisable()
    {
        ragdollController.Recovered -= OnRecover;
    }

    private void OnRecover()
    {
        IsKnockedOut = false;
        ResetOriginPosition();

    }

    private void PerformKnockout(Vector3 source, float impactForce)
    {
        ragdollController.Knockout(source, impactForce);
        GetOrigin().SetParent(ragdollController.GetHipsTransform());
        IsKnockedOut = true;
    }


    protected override bool IsDamagingBlocked()
    {
        return InBlockingWindow || IsDead ;
    }

    public override void TakeDamage(DamageData damageData, Transform source)
    {
        base.TakeDamage(damageData, source);

        if (IsDead) return;

        if (damageData.balanceDamageType == BalanceDamageType.Extreme && !IsKnockedOut)
        {

            Vector3 sourcePos = source != null ? source.position : self.position - self.forward;
            PerformKnockout(sourcePos, damageData.impactForce);
        }

        else
        {
            HandleGetDamaged(damageData.balanceDamageType);
        }

    }








}