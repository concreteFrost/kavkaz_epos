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
            BalanceDamageType balanceDamage =
       (BalanceDamageType)UnityEngine.Random.Range(1, 4);
            TakeDamage(20, balanceDamage, null);
        }
    }

    public override void TakeDamage(float damage, BalanceDamageType balanceDamage, Transform source)
    {
        if (isDead || isDamaged || motor.IsDodging) return;


        BalancePenalty = balanceDamage;

        isDamaged = true;

        statsController.ReduceHealth(damage);   
      

    }

    public override void Die()
    {
        isDead = true;

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

        isDead = false;

    }

    private IEnumerator RespawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }
}
