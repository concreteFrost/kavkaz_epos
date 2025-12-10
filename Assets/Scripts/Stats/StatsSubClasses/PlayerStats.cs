using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerStatsUI ui;
    IAttackSource inventory;
    ICharacterAnimator animator;
    private void Awake()
    {
        damagableId = GetInstanceID().ToString();
    }

    public void Init(IAttackSource src, ICharacterAnimator anim, PlayerStatsUI _ui)
    {
        base.InitializeStats();

        inventory = src;
        animator = anim;
        ui = _ui;

        ui.Init(this);
    }

    private void Update()
    {
        HandleStaminaRegen();   

        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(20, Random.Range(0, 1f));
        }

    }

    #region Health Control
    public override void TakeDamage(float damage, float balanceDamage)
    {

        animator.BalancePenalty = balanceDamage;

        if (!animator.IsShieldRaised)
        {
            animator.IsDamaged = true;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        ui.UpdateHealthSlider(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    #endregion

    public override void ReduceStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        ui.UpdateStaminaSlider(currentStamina);

        staminaRegenTimer = 0f;
    }

    public override void HandleStaminaRegen()
    {
        // Если стамина полная, ничего не делать
        if (currentStamina >= maxStamina)
            return;

        // Если игрок атакует, бежит, катится и т.п. — можно тоже отключать реген (если нужно)
        if (animator != null && animator.IsAttacking)
            return;

        // Считаем таймер
        staminaRegenTimer += Time.deltaTime;

        // Ждём N секунд
        if (staminaRegenTimer < staminaRegenDelay)
            return;

        // Когда таймер вышел — регеним стамину
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        ui.UpdateStaminaSlider(currentStamina);
    }

}
