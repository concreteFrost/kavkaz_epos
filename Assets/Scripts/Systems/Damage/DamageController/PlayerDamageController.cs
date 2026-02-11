using System.Collections;
using UnityEngine;

public class PlayerDamageController : BaseHumanoidDamageController
{

    private float damageCooldown = 0.7f;
    private bool damageBlocked = false;

    protected bool canTakeAnotherDamage = true;

    public void Init(
        PlayerMotor motor,
        CharacterStatsController stats,
        PlayerAnimatorController animatorController
        )
    {
        Debug.Log("Damage controller init");
        this.motor = motor; 
        this.stats = stats;
        this.animatorController = animatorController;
        CharacterType = stats.statsSO.characterType;

        if(aimPosition == null)
        {
            Debug.Log("no aim position assigned");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DamageData d = new DamageData
            {
                healthDamageMultiplier = 10f,
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

       HandleGetDamaged(damageData.balanceDamageType);
       StartCoroutine(DamageCooldownCoroutine());
    }

    protected override bool IsDamagingBlocked()
    {
        return IsDead || damageBlocked || motor.IsDodging;
    }


    private IEnumerator DamageCooldownCoroutine()
    {
        damageBlocked = true;
        yield return new WaitForSeconds(damageCooldown);
        damageBlocked = false;

    }


}
