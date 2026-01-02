using UnityEngine;

public abstract class CharacterStatsController : MonoBehaviour, ICharacterStatsModifier
{

   

    #region ICharacterStatsModifier Contract
    public abstract void ReduceStamina(float amount);
    public abstract void HandleStaminaRegen();
    #endregion

}
