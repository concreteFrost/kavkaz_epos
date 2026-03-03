using UnityEngine;

public abstract class BaseStatsController : MonoBehaviour , IStatsController
{
    public SpeedModel Speed;
    public HealthModel Health;
    public StaminaModel Stamina;
 
}