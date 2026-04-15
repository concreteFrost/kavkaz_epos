using System.Collections;
using UnityEngine;

public class PlayerDamageController : BaseHumanoidDamageController
{

    private float damageCooldown = 0.7f;
    private bool damageBlocked = false;

    public void Init(
        PlayerMotor motor,
        PlayerAnimatorController animatorController,
        CharacterStatsController statsController,
        CharacterStatsModifier statsModifier
        )
    {

        BaseInit(animatorController: animatorController, statsModifier: statsModifier, statsController: statsController, motor: motor, self: self);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DamageData d = new DamageData
            {
                finalDamage = 20f,
                balanceDamageType = BalanceDamageType.Extreme,
                impactForce = 20f
            };
            TakeDamage(d, null);
        }
    }

    public override void TakeDamage(DamageData damageData, Transform source)
    {
        base.TakeDamage(damageData, source);

        if (IsDead) return;

        if (ShouldEnterGameMode())
        {
            GameStateManager.GameStateChanged?.Invoke(GameState.Game);
        }

        HandleGetDamaged(damageData.balanceDamageType);
        StartCoroutine(DamageCooldownCoroutine());
    }

    protected override bool IsDamagingBlocked()
    {
        return IsDead || damageBlocked || InBlockingWindow;
    }


    private IEnumerator DamageCooldownCoroutine()
    {
        damageBlocked = true;
        yield return new WaitForSeconds(damageCooldown);
        damageBlocked = false;

    }

    private bool ShouldEnterGameMode()
    {

        if (GameStateManager.Instance.CurrentState == GameState.Game || 
            GameStateManager.Instance.CurrentState == GameState.Transition) return false;

        return true;    

    }


}