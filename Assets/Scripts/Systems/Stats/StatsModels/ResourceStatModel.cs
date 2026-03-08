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
    public float RegenRate { get; protected set; }

    protected float DefaultRegenDelay;
    protected float DefaultRegenRate;

    public event Action Depleted;

    protected void BaseInit(
        float baseValue,
        float minRegenDelay,
        float maxRegenDelay,
        float rate)
    {
        base.BaseInit(baseValue);

        MinRegenDelay = minRegenDelay;
        MaxRegenDelay = maxRegenDelay;
        RegenRate = rate;

        DefaultRegenDelay = MinRegenDelay;
        DefaultRegenRate = RegenRate;

        RegenTimer = 0;
    }


    public virtual void ReduceCurrent(float amount)
    {
        if (Current <= 0) return;

        Current -= amount;
        if (Current < 0) Current = 0;

        NotifyCurrentChange(Current);

        if (Current == 0)
            Depleted?.Invoke();
    }

    public virtual void IncreaseCurrent(float amount)
    {
        if (Current >= CurrentMax) return;

        Current += amount;
        Current = Mathf.Clamp(Current, 0, CurrentMax);

        NotifyCurrentChange(Current);
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

        Current += RegenRate * Time.deltaTime;
        Current = Mathf.Clamp(Current, 0, CurrentMax);

        NotifyCurrentChange(Current);
    }

    public void SetRegenDelay(float delay) => MinRegenDelay = delay;

    public void ResetCurrentRegenDelay() => MinRegenDelay = DefaultRegenDelay;

    public void ResetRegenRate() => RegenRate = DefaultRegenRate;

    public void ResetCurrent()
    {
        Current = CurrentMax;
        NotifyCurrentChange(Current);
    }

}