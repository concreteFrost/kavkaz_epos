using System.Collections;
using UnityEngine;

public class PlayerLifecycle : CharacterLifecycle
{
    
    PlayerInput input;

    public void Init(
        IDamagable damagable,CharacterStatsController statsController,PlayerInput input, CharacterStatsModifier statsModifier)
    {
        BaseInit(statsController,statsModifier,damagable);  
        this.input = input;
    }

  
    public override void Die()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;
        statsModifier.ClearAllStats();  
        input.controls.Disable();

        StartCoroutine(RespawnCoroutine());


    }

    public override void Respawn()
    {
        input.controls.Enable();
        statsController.ResetAllStats();

        damagable.IsDead = false;

    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(5f);
        Respawn();
    }
}
