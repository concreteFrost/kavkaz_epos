using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class LevelStatModel
{
    protected StatType statType;
    protected int currentLevel = 1;
    protected float baseValue;
    public float Current;
    public float CurrentMax;
    protected abstract float PerLevelBonus { get; }
    protected abstract float DiminishFactor { get; }

    public event Action Depleted;
    public event Action<float> CurrentChanged;
    public event Action<int, float> MaxChanged;

    protected Dictionary<string, float> tempModifiers = new Dictionary<string, float>();

    float CalculateTempMax()
    {
        float total = 0;

        foreach (var v in tempModifiers.Values)
            total += v;

        return total;
    }

    protected void BaseInit(float baseValue)
    {
        this.baseValue = baseValue;
        CurrentMax = baseValue;
        Current = CurrentMax;
    }

    public float CalculateNextLevel(int level)
    {
        currentLevel = Mathf.Max(1, level);

        float totalBonus = PerLevelBonus * (1 - Mathf.Pow(DiminishFactor, currentLevel - 1)) / (1 - DiminishFactor);

        return baseValue + totalBonus;
    }

    public void UpdateMaxAndCurrent(int level)
    {
        CurrentMax = CalculateNextLevel(level) + CalculateTempMax();
        CurrentMax = Mathf.Max(1, CurrentMax); // минимальное ограничение
        Current = CurrentMax;

        NotifyMaxChange(level, CurrentMax);
        NotifyCurrentChange(Current);
    }

    protected void RecalculateCurrentMax()
    {
        // считаем новый максимум
        CurrentMax = CalculateNextLevel(currentLevel) + CalculateTempMax();

        // минимальное ограничение
        CurrentMax = Mathf.Max(0, CurrentMax);

        // текущее не может быть выше максимума
        if (Current > CurrentMax)
            Current = CurrentMax;

        // уведомления для UI
        NotifyMaxChange(currentLevel, CurrentMax);
        NotifyCurrentChange(Current);
    }

    public void ChangeMax(string id, float val, OperationType operationType)
    {
        var calculatedAmount = operationType == OperationType.Positive ? val : -val;
        if (tempModifiers.ContainsKey(id)) tempModifiers[id] += calculatedAmount;
        else tempModifiers[id] = calculatedAmount;

        RecalculateCurrentMax();
    }   


    public void ResetCurrent()
    {
        Current = CurrentMax;
        NotifyCurrentChange(Current);
    }

    public void ResetMax(string effectId)
    {
        if (tempModifiers.Remove(effectId))
            RecalculateCurrentMax();
    }

    public virtual void ChangeCurrent(float amount, OperationType operationType)
    {
        float delta = operationType == OperationType.Positive ? amount : -amount;

        if (delta > 0 && Current >= CurrentMax) return;
        if (delta < 0 && Current <= 0) return;

        Current += delta;
        Current = Mathf.Clamp(Current, 0, CurrentMax);

        NotifyCurrentChange(Current);

        if (Current == 0)
            Depleted?.Invoke();
    }



    protected void NotifyMaxChange(int leve, float value) => MaxChanged?.Invoke(leve, value);

    public void NotifyCurrentChange(float amount) => CurrentChanged?.Invoke(amount);
}
