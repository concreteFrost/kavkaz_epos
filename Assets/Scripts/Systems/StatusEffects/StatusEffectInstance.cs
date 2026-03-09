using UnityEngine;

[System.Serializable]
public class StatusEffectInstance
{
    public readonly ContinuousStatusEffectSO data;

    private float defaultDuration;
    public float duration;
    public float accumulation = 1f;
    public float amount;
  
    public bool isActive;

    public StatusEffectInstance(ContinuousStatusEffectSO data, float amount)
    {
        this.data = data;

        duration = data.duration;
        defaultDuration = duration;
        accumulation = 0;
        isActive = false;
        this.amount = amount;

    }

    public void IncreaseDuration()
    {

        duration = defaultDuration;

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
            float finalAmount = amount * Time.deltaTime;
            data.Apply(stats,finalAmount);
        }


        return accumulation <=0f;
    }

}
