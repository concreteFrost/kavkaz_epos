using System.Collections;
using UnityEngine;

public class PlayerDamageController : BaseDamageController
{
    IHumanoidMovement motor;
    IHumanoidCombat combatController;
    IAttackSource inventory;
    
    PlayerInput input;
    
    protected bool canTakeAnotherDamage = true;


    public void Init(IHumanoidMovement motor, ICharacterStatsController statsController,HumanoidStats stats ,IHumanoidCombat combatController, IAttackSource inventory, PlayerInput input, string uniqueID)
    {
        this.motor = motor; 
        this.uniqueID = uniqueID; 
        this.statsController = statsController;
        this.combatController = combatController;
        this.inventory = inventory;
        this.input = input; 
        this.stats = stats;

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
            TakeDamage(20, UnityEngine.Random.Range(0, 1f), null);
        }
    }

    public override void TakeDamage(float damage, float balanceDamage, Transform source)
    {
        if (isDead || isDamaged || motor.IsDodging) return;

        balancePenalty = balanceDamage;

        isDamaged = balancePenalty > 0f;

        if (isDamaged)
        {
            StartCoroutine(DamageCooldownCoroutine(maxDamageCooldown));
        }

        statsController.ReduceHealth(damage);   
      

    }
    IEnumerator DamageCooldownCoroutine(float delay)
    {
        canTakeAnotherDamage = false;
        yield return new WaitForSeconds(delay);
        canTakeAnotherDamage = true;
    }


    public override void Die()
    {
        isDead = true;

        input.controls.Player.Disable();

        inventory.CurrentWeapon?.DropWeapon();
        inventory.ShieldWeapon?.ThrowShield();
        inventory.ResetWeapon();

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
