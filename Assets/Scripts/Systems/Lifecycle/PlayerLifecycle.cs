using System.Collections;
using UnityEngine;

public class PlayerLifecycle : CharacterLifecycle
{

    public void Init(
     BaseHumanoidDamageController damagable, CharacterStatsController statsController, CharacterStatsModifier statsModifier, Vector3 startingPosition, Transform self)
    {
        BaseInit(statsController, statsModifier, damagable, startingPosition, self);

    }

    public override void Die()
    {
        if (damagable.IsDead) return;

        damagable.IsDead = true;
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
