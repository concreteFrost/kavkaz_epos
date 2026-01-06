using System.Collections;
using UnityEngine;

public class PlayerDamageController : BaseDamageController
{
    IHumanoidCombat combatController;
    IAttackSource inventory;
    
    PlayerInput input;
    
    protected bool canTakeAnotherDamage = true;


    public void Init(ICharacterStatsController statsController,CharacterStats stats ,IHumanoidCombat combatController, IAttackSource inventory, PlayerInput input, string uniqueID)
    {
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

    public override void TakeDamage(float damage, float balanceDamage, IAttackSource source)
    {
        if (isDead || !canTakeAnotherDamage) return;

        balancePenalty = balanceDamage;

        if (!combatController.IsShieldRaised)
        {
            isDamaged = true;
        }

        statsController.ReduceHealth(damage);   
        StartCoroutine(DamageCooldownCoroutine(maxDamageCooldown));

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
