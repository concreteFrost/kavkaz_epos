using System;
using UnityEngine;


[System.Serializable]
public class StatusEffectInstance
{
    public ContinuousStatusEffectSO data;

    private float accumulationIncreaseMultiplier = 0.12f;
    private float accumulationDecreaseMultiplier = 0.01f;

    private float defaultDuration;
    public float duration;

    private float accumulation = 1f;

    public float Progress
    {
        get
        {
            if (data.useAccumulation)
                return accumulation;

            return duration / defaultDuration;
        }
    }

    public float DefaultDuration => defaultDuration;
    public float Accumulation => accumulation;

    public float amount;
  
    public bool isActive;

    public StatusEffectInstance(ContinuousStatusEffectSO data, float amount, float duration)
    {
        this.data = data;
        this.amount = amount;
        this.duration = duration;

        defaultDuration = duration;
        accumulation = 0;
        isActive = false;
       ;
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
        if (!data.useAccumulation)
        {
            isActive = true;
            duration -= dt;

            if (duration <= 0)
                return true;

            data.Tick(stats, amount * dt);

            return false;
        }

        // старая логика накопления
        duration -= dt;

        if (duration <= 0f)
        {
            accumulation -= dt * accumulationDecreaseMultiplier;
            duration = 0f;
        }

        if (!isActive && duration > 0)
        {
            accumulation += dt * accumulationIncreaseMultiplier;

            if (accumulation >= 1f)
            {
                accumulation = 1f;
                duration = defaultDuration;
                isActive = true;
            }
        }

        if (isActive)
        {
            accumulation -= dt * accumulationDecreaseMultiplier;
            duration = 0;

            data.Tick(stats, amount * dt);
        }

        return accumulation <= 0f;
    }

    public static StatusEffectInstance Load(
    ContinuousStatusEffectSO data,
    float amount,
    float duration,
    float defaultDuration,
    float accumulation,
    bool isActive)
    {
        var instance = new StatusEffectInstance(data, amount, duration);

        instance.defaultDuration = defaultDuration;
        instance.accumulation = accumulation;
        instance.isActive = isActive;   

        return instance;
    }


}


