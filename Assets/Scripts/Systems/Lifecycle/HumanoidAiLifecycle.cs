using System.Collections;
using UnityEngine;

public class HumanoidAiLifecycle : CharacterLifecycle
{
    IDamagable damagable;
    CharacterStatsController statsController;
    IRagdollController ragdollController;
    IBrain brain;

    public void Init(IDamagable damagable,CharacterStatsController statsController, IRagdollController ragdollController, IBrain brain)
    {
        this.damagable = damagable;
        this.statsController = statsController; 
        this.ragdollController = ragdollController;
        this.brain = brain; 

        statsController.Health.Depleted += Die;
    }

    private void OnDisable()
    {
        statsController.Health.Depleted -= Die;
    }
    public override void Die()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;

        if (!ragdollController.IsKnockedOut)
        {
            ragdollController.EnableRagdoll(Vector3.zero, 0);
        }
       
        brain.ForceStop();

        StartCoroutine(RespawnCoroutine());
       
    }

    public override void Respawn()
    {
        damagable.IsDead = false;

        ragdollController.DisableRagdoll();
        brain.SetInitialState();
        statsController.ResetAllStats();
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(7f);
        Respawn();
    }
}
