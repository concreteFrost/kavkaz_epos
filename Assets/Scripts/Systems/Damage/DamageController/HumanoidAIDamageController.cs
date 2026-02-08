
using UnityEngine;

public class HumanoidAIDamageController : BaseDamageController
{
    CapsuleCollider col;
    IRagdollController ragdollController;
    ITargetLocker targetLocker;
	public void Init(HumanoidDamageServices service)
	{
        this.motor = service.motor;
		this.statsController = service.statsModifier;
		this.stats =service.stats;
        this.ragdollController = service.ragdollController;
        this.col = service.col; 
        this.animatorController = service.animatorController;
	
        stats.Health.Depleted += Die;

        ragdollController.RecoveredInInvalidArea += OnInvalidRecover;
        ragdollController.Recovered += OnRecover;

        if (aimPosition == null)
        {
            Debug.Log("aim position on ai is not assigned");
        }

	}

    private void OnDisable()
    {
        stats.Health.Depleted -= Die;
        ragdollController.Recovered -= OnRecover;
        ragdollController.RecoveredInInvalidArea -= OnInvalidRecover;
    }

    protected void OnInvalidRecover()
    {
        if (IsDead) return;
        Die();
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
                healthDamageMultiplier = 10f,
                balanceDamageType = BalanceDamageType.Extreme,
                impactForce = 20f
            };
            TakeDamage(d, null);
        }
       
    }

  

    public override void TakeDamage(DamageData damageData, Transform source)
    {
        base.TakeDamage(damageData, source);

        if (damageData.balanceDamageType == BalanceDamageType.Extreme && !ragdollController.IsKnockedOut)
        {
            Vector3 sourcePos = source != null ? source.position : transform.position - transform.forward;
            PerformKnockout(sourcePos, damageData.impactForce);
        }

        else
        {
            HandleGetDamaged(damageData.balanceDamageType);
        }

    }

    private void PerformKnockout(Vector3 source, float impactForce)
    {
        //motor.ResetLockTarget();
        ragdollController.Knockout(source,impactForce);
        IsKnockedOut = true;
    }

    protected override bool IsDamagingBlocked()
    {
        return motor.IsDodging || IsDead;
    }

    public override void Die()
    {
        IsDead = true;

        col.enabled = false;

        ragdollController.ForceStop();
        ragdollController.EnableRagdoll(Vector3.zero,300);

    }




}
