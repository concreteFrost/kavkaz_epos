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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        DamageData d = new DamageData
    //        {
    //            healthDamageMultiplier = 10f,
    //            balanceDamageType = BalanceDamageType.Extreme,
    //            impactForce = 20f
    //        };
    //        TakeDamage(d, null);
    //    }
    //}

    public override void TakeDamage(DamageData damageData, Transform source)
    {
        base.TakeDamage(damageData, source);

        if (IsDead) return;

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


}