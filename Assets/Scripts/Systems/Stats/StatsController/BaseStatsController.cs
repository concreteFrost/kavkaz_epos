using UnityEngine;

public abstract class BaseStatsController : MonoBehaviour , IStatsController
{
    public SpeedModel Speed;
    public HealthModel Health;
    public StaminaModel Stamina;

    public IStatModel GetRequiredModel(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health: return Health;
            case StatType.Stamina: return Stamina;
            case StatType.Speed: return Speed;

            default: return null;
        }
    }

 
}