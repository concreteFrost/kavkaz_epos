using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class HumanoidAIDamageController : BaseDamageController
{
    NavMeshAgent agent;
    CapsuleCollider col;
    IHumanoidMovement motor;
    IRagdollController ragdollController;

	public void Init(HumanoidDamageControllerService service)
	{
        this.motor = service.motor;
		this.statsController = service.statsModifier;
		this.stats =service.stats;
        this.agent = service.agent; 
        this.ragdollController = service.ragdollController;
        this.col = service.col; 
		this.uniqueID =service.uniqueID;
       
        stats.Health.Depleted += Die;

        if(aimPosition == null)
        {
            Debug.Log("aim position on ai is not assigned");
        }

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

    private void OnDisable()
    {
        stats.Health.Depleted -= Die;   
    }

    public override void TakeDamage(DamageData damageData, Transform source )
    {
        if (motor.IsDodging || isDead)
            return;

        base.TakeDamage(damageData, source);

        if (damageData.balanceDamageType == BalanceDamageType.Extreme)
        {
            ragdollController.Knockout(damageData.impactForce,source);
        }
    }

    public override void Die()
    {
        isDead = true;

        col.enabled = false;

        ragdollController.ForceStop();
        ragdollController.EnableRagdoll();

     

    }

}
