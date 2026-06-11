using System;
using UnityEngine;

public class PlayerLifecycle : CharacterLifecycle
{

    public Action PlayerDied;
    PlayerFallController fallController;
    public void Init(
    BaseHumanoidDamageController damagable, CharacterStatsController statsController, CharacterStatsModifier statsModifier, Vector3 startingPosition, Transform self, PlayerFallController fallController)
    {
        BaseInit(statsController, statsModifier, damagable, startingPosition, self);
        this.fallController = fallController;   
   
    }

    public override void Die()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;

        PlayerDied?.Invoke();
        
        var currentSceneName = GameRunner.Instance.activeLevel.GetLevelName();
        SceneTransitionManager.Instance.TravelToLevel(currentSceneName, respawnPosition);


    }

    public override void Respawn(Vector3 pos)
    {
        damagable.IsDead = false;

        statsModifier.ClearAllStats();
        statsController.ResetAllStats();
        fallController.ResetLastGroundedPosition(pos); 

        SetStartingPosition(pos);
        ResetPosition();
       

    }



}
