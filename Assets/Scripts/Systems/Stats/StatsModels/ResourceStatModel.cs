using System;
using UnityEngine;

[System.Serializable]
public abstract class ResourceStatModel : LevelStatModel , IStatModifier
{
    protected ModifiedModelType modelType;
    public ModifiedModelType ModifiedModelType()=>modelType;   
    public float RegenTimer { get; protected set; }
    public float MinRegenDelay { get; protected set; }
    public float MaxRegenDelay { get; protected set; }
    public float CurrentRegenRate { get; protected set; }

    protected float TempRegenRate;

    protected float DefaultRegenDelay;
    protected float DefaultRegenRate;


    protected void BaseInit(
        float baseValue,
        float minRegenDelay,
        float maxRegenDelay,
        float rate)
    {
        base.BaseInit(baseValue);

        MinRegenDelay = minRegenDelay;
        MaxRegenDelay = maxRegenDelay;
        CurrentRegenRate = rate;

        DefaultRegenDelay = MinRegenDelay;
        DefaultRegenRate = CurrentRegenRate;

        RegenTimer = 0;
    }


    public void Regen()
    {
        if (Current >= CurrentMax)
        {
            Current = CurrentMax;
            return;
        }

        float regenDelay =
            Current <= 0.1f ? MaxRegenDelay : MinRegenDelay;

        RegenTimer += Time.deltaTime;

        if (RegenTimer < regenDelay)
            return;

        Current += CurrentRegenRate * Time.deltaTime;
        Current = Mathf.Clamp(Current, 0, CurrentMax);

        NotifyCurrentChange(Current);
    }

    public void ChangeRegenRate(string id, float val, OperationType operationType)
    {
        // вычисляем положительное или отрицательное изменение
        float delta = operationType == OperationType.Positive ? val : -val;

        // если эффект уже есть — суммируем
        if (tempModifiers.ContainsKey(id))
            tempModifiers[id] += delta;
        else
            tempModifiers[id] = delta;

        // пересчитываем текущую скорость с учётом всех модификаторов
        RecalculateRegenRate();
    }

    // отдельный метод пересчёта, чтобы избежать дублирования
    private void RecalculateRegenRate()
    {
        float total = 0f;
        foreach (var v in tempModifiers.Values)
            total += v;

        CurrentRegenRate = DefaultRegenRate + total;

        // минимальное ограничение
        if (CurrentRegenRate < 0)
            CurrentRegenRate = 0;
    }

    public void ResetRegenRate()
    {
        CurrentRegenRate = DefaultRegenRate;
        TempRegenRate = 0;
    }



}