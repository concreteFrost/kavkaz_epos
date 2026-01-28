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
            TakeDamage(10, BalanceDamageType.Extreme, null);
        }

       
    }

    private void OnDisable()
    {
        stats.Health.Depleted -= Die;   
    }

    public override void TakeDamage(float damage, BalanceDamageType balanceDamage, Transform source)
    {
        if (motor.IsDodging || isDead) return;

        if(balanceDamage == BalanceDamageType.Extreme && !isDamaged)
        {
            ragdollController.Knockout();

            StartCoroutine(ragdollController.Recover());
            //StartCoroutine(RagdollUtils.IsMoving())
        }
        base.TakeDamage(damage, balanceDamage, source);

     
    }

    public override void Die()
    {
        base.Die();
     
        StartCoroutine(PerformDeathCoroutine(4f));
    }

    IEnumerator PerformDeathCoroutine(float delay)
    {
        col.enabled = false;
        yield return new WaitForSeconds(delay);

        //StartCoroutine(ragdollController.EnableRagdoll());

    

      
    }
}
