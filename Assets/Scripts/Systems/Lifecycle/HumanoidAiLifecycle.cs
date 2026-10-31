using System.Collections;
using UnityEngine;

public class HumanoidAiLifecycle : CharacterLifecycle
{
   
    IRagdollController ragdollController;
    IBrain brain;
    PointsEmitter pointsEmitter;
    CharacterLootDistributer distributer;
    EnemyFOVController fovController;
    IHumanoidMeleeCombat combatController;
    IEmitter emitter;
    HumanoidAIMotor motor;

    public void Init(HumanoidAIDamageController damagable,CharacterStatsController statsController, CharacterStatsModifier statsModifier,CharacterAudioManager audioManager ,IRagdollController ragdollController, IBrain brain, Vector3 startingPosition, Transform self, PointsEmitter pointsEmitter, CharacterLootDistributer distributer, EnemyFOVController fovController, IHumanoidMeleeCombat combatController, IEmitter emitter, HumanoidAIMotor motor)
    {
        UnsubscribeRagdollEvents();
        BaseInit(statsController, statsModifier, audioManager ,damagable, startingPosition, self);
        this.ragdollController = ragdollController;
        this.brain = brain; 
        this.distributer = distributer; 
        this.fovController = fovController; 
        this.pointsEmitter = pointsEmitter;
        this.combatController = combatController;
        this.emitter = emitter;
        this.motor = motor;

        ragdollController.KnockedOut += OnKnockedOut;
        ragdollController.Recovered += OnRecovered;

    }

  

    private void OnKnockedOut()
    {
        if (damagable.IsDead)
            return;

        // Нокаут прерывает текущие действия, но не запускает путь смерти и респавна.
        brain.SetPaused(true);
        combatController.ForceAttackCancel();
        combatController.CancelBlock();
        emitter.EndEmit();
        motor.ResetCombatMovement();
    }

    private void OnRecovered()
    {
        if (!damagable.IsDead)
            brain.SetPaused(false);
    }

    private void UnsubscribeRagdollEvents()
    {
        if (ragdollController == null)
            return;

        ragdollController.KnockedOut -= OnKnockedOut;
        ragdollController.Recovered -= OnRecovered;
    }

    private void OnDestroy()
    {
        UnsubscribeRagdollEvents();
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

        audioManager.PlayDeath();
        damagable.IsDead = true;

        ragdollController.ForceStop();
        ragdollController.EnableRagdoll(Vector3.zero, 0);

        brain.ForceStop();

        NotifyAlive(false);
       
        CharactersManager.CharacterStatesUpdated?.Invoke(); 

    }

    public override void Respawn(Vector3 pos)
    {
        ragdollController.ForceStop();
        ragdollController.IsKnockedOut = false;
        damagable.IsKnockedOut = false;
        damagable.IsDead = false;
        damagable.ResetOriginPosition();
      
        ResetPosition();
        
        ragdollController.DisableRagdoll();
        statsModifier.ClearAllStats();
        brain.ForceStop();
        brain.SetInitialState();
        statsController.ResetAllStats();
        fovController.ResetLockedTarget();

        NotifyAlive(true);
        CharactersManager.CharacterStatesUpdated?.Invoke();
    }

}
