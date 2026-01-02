using System.Collections;
using UnityEngine;

public class PlayerStatsController : CharacterStatsController
{
    PlayerStats stats;
    PlayerStatsUI ui;
    PlayerInput input;
    PlayerCombatInventory inventory;
    HumanoidCombatController combatController;

    PlayerLifeCicle lifeCicle;

    public void Init(PlayerStatsModifierServiceProvider provider)
    {
        stats = provider.stats;
        ui = provider.ui;
        input = provider.input;
        inventory = provider.inventory;
        combatController = provider.combatController;
        uniqueID = provider.uniqueId;

        ui.Init(stats);

        lifeCicle = new PlayerLifeCicle(inventory, stats, input, ui);
        


        Subscribe();
    }


    void Subscribe()
    {
        stats.Health.Depleted += Die;
        stats.Health.Changed += OnHealthChanged;

        stats.Stamina.Changed += OnStaminaChanged;
    }

    private void OnDisable()
    {
        stats.Health.Depleted -= Die;
        stats.Health.Changed -= OnHealthChanged;
        stats.Stamina.Changed -= OnStaminaChanged;
    }

    private void Update()
    {
        HandleStaminaRegen();

        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20, UnityEngine.Random.Range(0, 1f), null);
        }

         

    }

    #region Death and Respawn
    public override void Die()
    {
        isDead = true;
        lifeCicle.Die();

        StartCoroutine(RespawnCoroutine(5f));

    }

    public void Respawn()
    {
        lifeCicle.Respawn();
        isDead = false;
    }

    private IEnumerator RespawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }

    #endregion

    #region Health Control
    public override void TakeDamage(float damage, float balanceDamage, IAttackSource source)
    {
        if (isDead || !canTakeAnotherDamage) return;

        balancePenalty = balanceDamage;

        if (!combatController.IsShieldRaised)
        {
            isDamaged = true;
        }

        stats.Health.Damage(damage);
        StartCoroutine(DamageCooldownCoroutine(maxDamageCooldown));

    }

    IEnumerator DamageCooldownCoroutine(float delay)
    {
        canTakeAnotherDamage = false;
        yield return new WaitForSeconds(delay);
        canTakeAnotherDamage = true;
    }

    void OnHealthChanged(float amount) => ui.UpdateHealthSlider(amount);

    #endregion

    public override void ReduceStamina(float amount)
    {
        stats.Stamina.Reduce(amount);
    }
    public override void HandleStaminaRegen()
    {
        stats.Stamina.Regen();
    }

    private void OnStaminaChanged(float amount)
    {
        ui.UpdateStaminaSlider(amount); 
    }



}