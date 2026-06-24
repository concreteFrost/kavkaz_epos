using System.Collections;
using UnityEngine;

public class PlayerDamageController : BaseHumanoidDamageController
{

    protected override float DamageCooldown() => 0.7f;


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
                finalDamage = 100f,
                balanceDamageType = BalanceDamageType.Extreme,
                impactForce = 20f
            };
            TakeDamage(d, null);
        }
    }

    public override void TakeDamage(DamageData damageData, IAttackSource source)
    {
        if (IsDamagingBlocked()) return;

        base.TakeDamage(damageData, source);

        HandleGetDamaged(damageData.balanceDamageType);

        StartCoroutine(DamageCooldownCoroutine());

        if (ShouldForceEnterGameMode())
        {
            GameStateManager.Instance.SetState(GameState.Game);
        }
    }

    protected override bool IsDamagingBlocked()
    {
        return IsDead || damageBlocked || InBlockingWindow;
    }


    private bool ShouldForceEnterGameMode()
    {
        if (GameStateManager.Instance == null) return false;

        if (GameStateManager.Instance.CurrentState == GameState.Game || 
            GameStateManager.Instance.CurrentState == GameState.Transition) return false;

        return true;    

    }

}