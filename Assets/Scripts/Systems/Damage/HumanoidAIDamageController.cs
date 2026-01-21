using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class HumanoidAIDamageController : BaseDamageController
{
    NavMeshAgent agent;
    CapsuleCollider col;
    IHumanoidMovement motor;

	public void Init(HumanoidDamageControllerService service)
	{
        this.motor = service.motor;
		this.statsController = service.statsModifier;
		this.stats =service.stats;
        this.agent = service.agent; 
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
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    TakeDamage(10, 1, null);
        //}
    }

    private void OnDisable()
    {
        stats.Health.Depleted -= Die;   
    }

    public override void TakeDamage(float damage, BalanceDamageType balanceDamage, Transform source)
    {
        if (motor.IsDodging) return;

        base.TakeDamage(damage, balanceDamage, source);

     
    }

    public override void Die()
    {
        base.Die();
        agent.enabled = false;
        StartCoroutine(DisableColliderCoroutine(3f));
    }

    IEnumerator DisableColliderCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        col.enabled = false;
    }
}
