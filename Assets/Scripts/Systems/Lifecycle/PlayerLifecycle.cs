using System.Collections;
using UnityEngine;

public class PlayerLifecycle : CharacterLifecycle
{
    IDamagable damagable;
    CharacterStatsController statsController;
    PlayerInput input;


    public void Init(
        IDamagable damagable,CharacterStatsController statsController,PlayerInput input)
    {

        this.damagable = damagable;
        this.statsController = statsController;
        this.input = input;

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
