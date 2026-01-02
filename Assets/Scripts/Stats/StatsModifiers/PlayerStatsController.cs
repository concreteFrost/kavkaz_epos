
public class PlayerStatsController : CharacterStatsController
{
    public CharacterStats stats;

    public void Init(PlayerStatsControllerService provider)
    {
        stats = provider.stats;
    }

    public void ResetAllStats()
    {
        stats.Health.ResetHealth(stats.maxHealth);
        stats.Stamina.ResetStamina(stats.maxStamina);
    }

    private void Update()
    {
        HandleStaminaRegen();
    }

    #region Health Control
    public void ReduceHealth(float damage)
    {
        stats.Health.Damage(damage);
    }

    #endregion

    #region Stamina Control
    public override void ReduceStamina(float amount)
    {
        stats.Stamina.Reduce(amount);
    }
    public override void HandleStaminaRegen()
    {
        stats.Stamina.Regen();
    }
    #endregion
}