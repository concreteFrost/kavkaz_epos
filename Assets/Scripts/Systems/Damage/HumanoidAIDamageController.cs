
using UnityEngine;

public class HumanoidAIDamageController : BaseDamageController
{
    CapsuleCollider col;
    HumanoidAIMotor motor;
    BaseHumanoidAnimatorController animatorController;
    IRagdollController ragdollController;

	public void Init(HumanoidDamageServices service)
	{
        this.motor = service.motor;
		this.statsController = service.statsModifier;
		this.stats =service.stats;
        this.ragdollController = service.ragdollController;
        this.col = service.col; 
		this.uniqueID =service.uniqueID;
        this.animatorController = service.animatorController;
       
        stats.Health.Depleted += Die;
        //ragdollController.Recovered += OnRecover;
        ragdollController.RecoveredInInvalidArea += OnInvalidRecover;

        if(aimPosition == null)
        {
            Debug.Log("aim position on ai is not assigned");
        }

	}


    //private void OnRecover()
    //{
    //    Debug.Log("recovered");
    //    ragdollController.IsKnockedOut = false;   
    //}

    protected void OnInvalidRecover()
    {
        if (IsDead) return;
        Die();
    }

    private void OnDisable()
    {
        stats.Health.Depleted -= Die;
        //ragdollController.Recovered -= OnRecover;
        ragdollController.RecoveredInInvalidArea -= OnInvalidRecover;
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
            PerformKnockout(damageData.impactForce, source);
        }

    }

    private void PerformKnockout(float impactForce, Transform source)
    {
        motor.ResetLockTarget(); //предотвращает деформацию тела при подьеме
        ragdollController.Knockout(impactForce, source);
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
        ragdollController.EnableRagdoll();

    }




}
