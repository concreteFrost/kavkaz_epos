using UnityEngine;

[System.Serializable]
public class StatusEffectInstance
{
    public readonly StatusEffectData data;

    public float duration;
    public float accumulation;
    public bool isActive;

    public StatusEffectInstance(StatusEffectData data)
    {
        this.data = data;
        duration = data.duration;
        accumulation = 0;
        isActive = false;
    }

    public void IncreaseDuration()
    {
        
        duration += 0.2f;

    }

    /// <summary>
    /// Tick вызывается каждый кадр.
    /// </summary>
    public bool Tick(float dt, CharacterStatsController stats)
    {
        duration -= dt; 
        

        if (duration <= 0f)
        {
            accumulation -= dt * data.accumulationDecreaseMultiplier;
            duration = 0f;  
           
        }

        if (!isActive && duration > 0)
        {
            
            accumulation += dt * data.accumulationIncreaseMultiplier;
            if (accumulation >= 1f)
            {
                accumulation = 1f;
                duration = data.duration;
                isActive = true;
                
            }
        }

        if (isActive)
        {
            accumulation -= dt * data.accumulationDecreaseMultiplier;
            duration = 0;
            switch (data.type)
            {
                case SideEffectType.Burn:
                case SideEffectType.Poison:
                    stats.Health.Reduce(data.statsAffectMultiplier * dt);
                    break;
            }
        }


        return accumulation <=0f;
    }
}

[System.Serializable]
public struct StatusEffectData
{
    public SideEffectType type;
    public float duration;
    public float statsAffectMultiplier;
    public float accumulationIncreaseMultiplier;
    public float accumulationDecreaseMultiplier;
  
}
