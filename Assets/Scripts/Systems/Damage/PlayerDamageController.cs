using System.Collections;
using UnityEngine;

public class PlayerDamageController : BaseDamageController
{
    IHumanoidMovement motor;
    IHumanoidCombat combatController;
    ICombatInventory combatInventory;
    
    PlayerInput input;

    protected bool canTakeAnotherDamage = true;


    public void Init(PlayerDamageControllerService service)
    {

        this.motor = service.motor; 
        this.uniqueID = service.uid;
        this.stats = service.stats;
        this.statsController = service.statsController;
        this.combatController = service.combatController;
        this.combatInventory = service.attackSource;
        this.input = service.input; 
      

        stats.Health.Depleted += Die;

        characterType = CharacterType.Player;

        if(aimPosition == null)
        {
            Debug.Log("no aim position assigned");
        }
    }


    private void OnDisable()
    {
        stats.Health.Depleted -= Die;   
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
        if (IsDead || IsDamaged || motor.IsDodging) return;


        BalancePenalty = damageData.balanceDamageType;

        IsDamaged = true;

        statsController.ReduceHealth(damageData.healthDamageMultiplier);   
      

    }

    public override void Die()
    {
        IsDead = true;

        input.controls.Player.Disable();

        combatInventory.CurrentWeapon?.DropWeapon();
        combatInventory.ShieldWeapon?.ThrowShield();
        combatInventory.ResetWeapon();

        StartCoroutine(RespawnCoroutine(5f));
    }

    public void Respawn()
    {

        input.controls.Player.Enable();
        statsController.ResetAllStats();

        IsDead = false;

    }

    private IEnumerator RespawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }
}
