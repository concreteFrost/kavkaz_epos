using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ResourceStatModel : LevelStatModel , IModifiedStat
{
    protected ModifiedModelType modelType;
    public ModifiedModelType ModifiedModelType()=>modelType;
    protected override float PerLevelBonus => 10f;
    protected override float DiminishFactor => 0.95f;

    public float RegenTimer { get; protected set; }
    public float MinRegenDelay { get; protected set; }
    public float MaxRegenDelay { get; protected set; }
    public float CurrentRegenRate { get; protected set; }

    protected float TempRegenRate;

    protected float DefaultRegenDelay;
    protected float DefaultRegenRate;

    protected Dictionary<string, float> regenModifiers = new Dictionary<string, float>();


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
        float delta = operationType == OperationType.Positive ? val : -val;

        if (regenModifiers.ContainsKey(id))
            regenModifiers[id] += delta;
        else
            regenModifiers[id] = delta;

        RecalculateRegenRate();
    }

    // отдельный метод пересчёта, чтобы избежать дублирования
    private void RecalculateRegenRate()
    {
        float total = 0f;
        foreach (var v in regenModifiers.Values)
            total += v;

        CurrentRegenRate = DefaultRegenRate + total;

        if (CurrentRegenRate < 0)
            CurrentRegenRate = 0;
    }

    public void ResetRegenRate(string id)
    {
        if (regenModifiers.Remove(id))
            RecalculateRegenRate();
    }



}