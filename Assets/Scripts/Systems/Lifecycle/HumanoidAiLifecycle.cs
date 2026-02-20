using System.Collections;
using UnityEngine;

public class HumanoidAiLifecycle : CharacterLifecycle
{
   
    IRagdollController ragdollController;
    IBrain brain;

    public void Init(IDamagable damagable,CharacterStatsController statsController, CharacterStatsModifier statsModifier, IRagdollController ragdollController, IBrain brain)
    {
        BaseInit(statsController,statsModifier,damagable);
        this.ragdollController = ragdollController;
        this.brain = brain; 

    }

    public override void Die()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;

        if (!ragdollController.IsKnockedOut)
        {
            ragdollController.EnableRagdoll(Vector3.zero, 0);
        }
       
        statsModifier.ClearAllStats();
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
