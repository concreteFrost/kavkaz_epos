
public interface ICharacterStatsModifier
{
    void ResetAllStats();
    void ReduceHealth(float amount);
    void ReduceStamina(float amount);
    void HandleStaminaRegen();

}
