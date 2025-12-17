using UnityEngine;

public interface ICharacterStatsModifier
{
    public abstract void ReduceStamina(float amount);
    public abstract void HandleStaminaRegen();
}
