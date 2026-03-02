using System;
using UnityEngine;

public abstract class BaseStatModel : IStatModel
{
    [SerializeField] protected int level;

    public event Action<float> Changed;
    public event Action<float> MaxChanged;

    protected StatType statType;
    public int CurrentLevel()
    {
        return level;
    }
    
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

    public void UpdateMaxAndCurrent(int level, float baseValue)
    {
        this.level = level;
        CurrentMax = Calculate(this.level, baseValue);
        Current = CurrentMax;
        NotifyChange(CurrentMax);
        NotifyMaxChange(CurrentMax);
    }

    public void UpdateMax(int level, float baseValue)
    {
        this.level = level;
        CurrentMax = Calculate(this.level, baseValue);

        NotifyMaxChange(CurrentMax);
    }

    protected void NotifyChange(float amount) => Changed?.Invoke(amount);   

    protected void NotifyMaxChange(float amount) => MaxChanged?.Invoke(amount);

    
}