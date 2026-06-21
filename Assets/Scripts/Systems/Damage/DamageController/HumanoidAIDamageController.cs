
using System;
using UnityEngine;


public class HumanoidAIDamageController : BaseHumanoidDamageController
{
    protected override float DamageCooldown() => 0.3f;

    IRagdollController ragdollController;

    public static Action<IDamagable> NotifySource;

    public void Init(
        Transform self,
        IHumanoidMovement motor,
        CharacterStatsController statsController,
        CharacterStatsModifier statsModifier,
        IRagdollController ragdollController,
        BaseHumanoidAnimatorController animatorController,
        AiHealthUI healthUI
        )
    {
        BaseInit(animatorController: animatorController, statsModifier: statsModifier, statsController: statsController, motor: motor, self: self);

        this.ragdollController = ragdollController;
        this.HealthProviderUI = healthUI;

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

    public override void PerformKnockout(Vector3 source, float impactForce)
    {
        ragdollController.Knockout(source, impactForce);
        GetOrigin().SetParent(ragdollController.GetHipsTransform());
      
        IsKnockedOut = true;
    }


    protected override bool IsDamagingBlocked()
    {
        return InBlockingWindow || IsDead ;
    }

    public override void TakeDamage(DamageData damageData, IAttackSource source)
    {

        if (IsDamagingBlocked()) return;

        NotifySource?.Invoke(this);

        base.TakeDamage(damageData, source);

        if (damageData.balanceDamageType == BalanceDamageType.Extreme && !IsKnockedOut)
        {

            Vector3 sourcePos = source != null ? source.Source().transform.position : self.position - self.forward;
            PerformKnockout(sourcePos, damageData.impactForce);
        }

        else
        {
            HandleGetDamaged(damageData.balanceDamageType);
           
        }

        //StartCoroutine(DamageCooldownCoroutine());

    }








}