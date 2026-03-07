using System;
using UnityEngine;

public abstract class BaseStatModel : IStatModel
{

    public event Action<float> Changed;
    public event Action<int,float> MaxChanged;

    protected StatType statType;

    protected float baseValue;
    
    public float Current;
    public float CurrentMax { get; set; }

    public StatType StatType() => statType;

    protected abstract float PerLevelBonus { get; }
    protected abstract float DiminishFactor { get; }

    float Calculate(int level, float baseValue)
    {
        int clampedLevel = Mathf.Max(1, level);

        float totalBonus = 0f;

        for (int i = 1; i < clampedLevel; i++)
        {
            float levelBonus = PerLevelBonus * Mathf.Pow(DiminishFactor, i - 1);
            totalBonus += Mathf.Floor(levelBonus);
        }

        return baseValue + totalBonus;
    }

    public virtual void UpdateMaxAndCurrent(int level)
    {
        
        CurrentMax = Calculate(level, baseValue);
        Current = CurrentMax;
      
        NotifyMaxChange(level, CurrentMax);
        NotifyChange(CurrentMax);
    }

    public virtual void UpdateMax(int level)
    {
        CurrentMax = Calculate(level, baseValue);
        NotifyMaxChange(level, CurrentMax);
    }

    protected void NotifyChange(float amount) => Changed?.Invoke(amount);   

    protected void NotifyMaxChange(int level, float amount) => MaxChanged?.Invoke(level,amount);

    public abstract void ReduceCurrent(float value);

    public abstract void IncreaseCurrent(float value);
}