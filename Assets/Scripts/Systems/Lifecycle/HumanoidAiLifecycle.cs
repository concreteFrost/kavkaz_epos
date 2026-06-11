using System.Collections;
using UnityEngine;

public class HumanoidAiLifecycle : CharacterLifecycle
{
   
    IRagdollController ragdollController;
    IBrain brain;
    PointsEmitter pointsEmitter;
    CharacterLootDistributer distributer;
    EnemyFOVController fovController;

    public void Init(HumanoidAIDamageController damagable,CharacterStatsController statsController, CharacterStatsModifier statsModifier, IRagdollController ragdollController, IBrain brain, Vector3 startingPosition, Transform self, PointsEmitter pointsEmitter, CharacterLootDistributer distributer, EnemyFOVController fovController)
    {
        BaseInit(statsController, statsModifier, damagable, startingPosition, self);
        this.ragdollController = ragdollController;
        this.brain = brain; 
        this.distributer = distributer; 
        this.fovController = fovController; 
        this.pointsEmitter = pointsEmitter;

    }


    public override void Die()
    {
        PerformDeath();

        fovController.ResetLockedTarget();  
        statsModifier.ClearAllStats();
        pointsEmitter.DropPoints();
        distributer.HandleLootGenerate(damagable.GetOrigin().transform.position);

    }

    public void PerformDeath()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;

        if (!ragdollController.IsKnockedOut)
        {
            ragdollController.EnableRagdoll(Vector3.zero, 0);
        }

        brain.ForceStop();

        CharactersManager.CharacterStatesUpdated?.Invoke(); 

        //StartCoroutine(RespawnCoroutine());
    }

    public override void Respawn(Vector3 pos)
    {
        damagable.IsDead = false;
        damagable.ResetOriginPosition();
        ResetPosition();
        
        ragdollController.DisableRagdoll();
        statsModifier.ClearAllStats();
        brain.SetInitialState();
        statsController.ResetAllStats();

        CharactersManager.CharacterStatesUpdated?.Invoke();
    }

}
