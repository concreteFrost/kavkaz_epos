using System.Collections;
using UnityEngine;

public class PlayerStatsModifier : CharacterStatsModifier
{
    PlayerStats stats;
    PlayerStatsUI ui;
    PlayerInput input;
    PlayerCombatInventory inventory;
    PlayerCombatController combatController;

    public void Init(PlayerStatsModifierServiceProvider provider)
    {
        stats = provider.stats;
        ui = provider.ui;
        input = provider.input;
        inventory = provider.inventory;
        combatController = provider.combatController;
        uniqueID = provider.uniqueId;

        ui.Init(stats);
    }

    private void Update()
    {
        HandleStaminaRegen();

        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20, UnityEngine.Random.Range(0, 1f));
        }

    }

    #region Death and Respawn
    public override void Die()
    {
        base.Die();
        input.controls.Player.Disable();

        inventory.CurrentWeapon?.DropWeapon();
        inventory.ShieldWeapon?.ThrowShield();
        inventory.ResetWeapon();

        StartCoroutine(RespawnCoroutine(5f));

    }

    public void Respawn()
    {

        input.controls.Player.Enable();

        stats.currentHealth = stats.maxHealth;
        stats.currentStamina = stats.maxStamina;

        ui.UpdateHealthSlider(stats.currentHealth);
        ui.UpdateStaminaSlider(stats.currentStamina);

        isDead = false;

    }

    IEnumerator RespawnCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Respawn();
    }
    #endregion

    #region Health Control
    public override void TakeDamage(float damage, float balanceDamage)
    {
        if (isDead) return;

        balancePenalty = balanceDamage;

        if (!combatController.isShieldRaised)
        {
            isDamaged = true;
        }

        stats.currentHealth -= damage;
        stats.currentHealth = Mathf.Clamp(stats.currentHealth, 0f, stats.maxHealth);

        ui.UpdateHealthSlider(stats.currentHealth);

        if (stats.currentHealth <= 0)
        {
            Die();
        }
    }

    #endregion

    public override void ReduceStamina(float amount)
    {
        stats.currentStamina -= amount;
        stats.currentStamina = Mathf.Clamp(stats.currentStamina, 0, stats.maxStamina);

        ui.UpdateStaminaSlider(stats.currentStamina);

        stats.staminaRegenTimer = 0f;
    }

    public override void HandleStaminaRegen()
    {
        // Если стамина полная, ничего не делать
        if (stats.currentStamina >= stats.maxStamina)
            return;

        // Если игрок атакует, бежит, катится и т.п. — можно тоже отключать реген (если нужно)
        if (combatController.isAttacking)
            return;

        // Считаем таймер
        stats.staminaRegenTimer += Time.deltaTime;

        // Ждём N секунд
        if (stats.staminaRegenTimer < stats.staminaRegenDelay)
            return;

        // Когда таймер вышел — регеним стамину
        stats.currentStamina += stats.staminaRegenRate * Time.deltaTime;
        stats.currentStamina = Mathf.Clamp(stats.currentStamina, 0, stats.maxStamina);

        ui.UpdateStaminaSlider(stats.currentStamina);
    }

    protected override void ResetBalance()
    {
        stats.currentBalance = 0f;
    }
}
