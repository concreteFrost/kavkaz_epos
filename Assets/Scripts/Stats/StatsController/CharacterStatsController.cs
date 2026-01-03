
using UnityEngine;

public class CharacterStatsController : MonoBehaviour, ICharacterStatsModifier
{

    public CharacterStats stats;

    public void Init(HumanoidStatsControllerService provider)
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
    public void ReduceStamina(float amount)
    {
        stats.Stamina.Reduce(amount);
    }
    public void HandleStaminaRegen()
    {
        stats.Stamina.Regen();
    }

    #endregion
}
