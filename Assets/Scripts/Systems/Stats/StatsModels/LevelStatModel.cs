using System;
using UnityEngine;

[System.Serializable]   
public abstract class LevelStatModel
{
    protected StatType statType;
    protected float baseValue;
    public float Current;
    public float CurrentMax { get;  set; }
    protected abstract float PerLevelBonus { get; }
    protected abstract float DiminishFactor { get; }

    public event Action<float> CurrentChanged;
    public event Action<int, float> MaxChanged;

    protected void BaseInit(float baseValue)
    {
        this.baseValue = baseValue;
        CurrentMax = baseValue;
        Current = CurrentMax;
    }

    public float CalculateNextLevel(int level)
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


    public void UpdateMaxAndCurrent(int level)
    {
        CurrentMax = CalculateNextLevel(level);
        Current = CurrentMax;

        NotifyMaxChange(level, CurrentMax);
        NotifyCurrentChange(Current);
    }

    protected void NotifyMaxChange(int leve, float value) => MaxChanged?.Invoke(leve, value);

    protected void NotifyCurrentChange(float amount) => CurrentChanged?.Invoke(amount);
}
