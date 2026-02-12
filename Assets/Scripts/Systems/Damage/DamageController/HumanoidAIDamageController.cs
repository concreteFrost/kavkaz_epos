
using UnityEngine;

public class HumanoidAIDamageController : BaseHumanoidDamageController
{

    IRagdollController ragdollController;

	public void Init(
        Transform self,
        IHumanoidMovement motor,
        CharacterStatsController statsController,
        IRagdollController ragdollController,
        BaseHumanoidAnimatorController animatorController
        )
	{
        this.self = self;
        this.motor = motor;
        this.stats = statsController;
        this.ragdollController = ragdollController;
        this.animatorController =animatorController;
        
        CharacterType = stats.statsSO.characterType;

        ragdollController.Recovered += OnRecover;

        if (aimPosition == null)
        {
            Debug.Log("aim position on ai is not assigned");
        }

	}

    private void OnDisable()
    {
        ragdollController.Recovered -= OnRecover;
    }

    private void OnRecover()
    {
        IsKnockedOut = false;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            DamageData d = new DamageData
            {
                healthDamageMultiplier = 30f,
                balanceDamageType = BalanceDamageType.Extreme,
                impactForce = 20f
            };
            TakeDamage(d, null);
        }
       
    }

    protected override bool IsDamagingBlocked()
    {
        return motor.IsDodging || IsDead;
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

    private void PerformKnockout(Vector3 source, float impactForce)
    {
        ragdollController.Knockout(source,impactForce);
        IsKnockedOut = true;
    }







}
