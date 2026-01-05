using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class HumanoidAIDamageController : BaseDamageController
{
    NavMeshAgent agent;
    CapsuleCollider col;
	public void Init(ICharacterStatsController statsModifier, CharacterStats stats, NavMeshAgent agent,CapsuleCollider col,  string uniqueID)
	{
		this.statsController = statsModifier;
		this.stats =stats;
        this.agent = agent; 
        this.col = col; 
		this.uniqueID =uniqueID;

        stats.Health.Depleted += Die;


	}

    private void OnDisable()
    {
        stats.Health.Depleted -= Die;   
    }

    public override void TakeDamage(float damage, float balanceDamage, IAttackSource source)
    {
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
