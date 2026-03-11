using System;
using UnityEngine;

[System.Serializable]
public class StatusEffectInstance
{
    public readonly ContinuousStatusEffectSO data;

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
    public float amount;
  
    public bool isActive;

    public StatusEffectInstance(ContinuousStatusEffectSO data, float amount, float duration)
    {
        this.data = data;

        this.duration = duration;
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
            accumulation -= dt * data.accumulationDecreaseMultiplier;
            duration = 0f;
        }

        if (!isActive && duration > 0)
        {
            accumulation += dt * data.accumulationIncreaseMultiplier;

            if (accumulation >= 1f)
            {
                accumulation = 1f;
                duration = defaultDuration;
                isActive = true;
            }
        }

        if (isActive)
        {
            accumulation -= dt * data.accumulationDecreaseMultiplier;
            duration = 0;

            data.Tick(stats, amount * dt);
        }

        return accumulation <= 0f;
    }
}
