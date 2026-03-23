using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLifecycle : CharacterLifecycle
{


    public void Init(
     IDamagable damagable, CharacterStatsController statsController, CharacterStatsModifier statsModifier, Vector3 startingPosition, Transform self)
    {
        BaseInit(statsController, statsModifier, damagable, startingPosition, self);

    }


    public override void Die()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;
        statsModifier.ClearAllStats();
        GameStateManager.GameStateChanged?.Invoke(GameState.Transition);

        StartCoroutine(RespawnCoroutine());


    }

    public override void Respawn()
    {
        GameStateManager.GameStateChanged?.Invoke(GameState.Game);
        statsModifier.ClearAllStats();
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
