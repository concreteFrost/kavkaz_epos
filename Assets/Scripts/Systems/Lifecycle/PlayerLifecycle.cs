using System.Collections;
using UnityEngine;

public class PlayerLifecycle : CharacterLifecycle
{
    
    PlayerInput input;

    public void Init(
        IDamagable damagable,CharacterStatsController statsController,PlayerInput input, CharacterStatsModifier statsModifier, Vector3 startingPosition, Transform self)
    {
        BaseInit(statsController,statsModifier,damagable,startingPosition,self);  
        this.input = input;
    }

  
    public override void Die()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;
        statsModifier.ClearAllStats();  
        input.DisableInput();

        StartCoroutine(RespawnCoroutine());


    }

    public override void Respawn()
    {
        input.EnableInput();
        statsController.ResetAllStats();

        ResetPosition();

        damagable.IsDead = false;

    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Respawn();
    }
}
