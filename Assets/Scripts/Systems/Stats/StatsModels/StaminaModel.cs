using System;
using UnityEngine;

[System.Serializable]   
public class StaminaModel : BaseStatModel
{
    public float RegenTimer { get; private set; }
    public float MinRegenDelay { get; private set; }
    public float MaxRegenDelay { get; private set; }
    public float RegenRate { get; private set; }

    private float DefaultRegenDelay;
    private float DefaultRegenRate;

    protected override float PerLevelBonus => 10f;
    protected override float DiminishFactor => 0.85f;

    public StaminaModel(float baseStamina, float minRegenDelay, float maxRegenDelay, float rate)
	{
        statType = global::StatType.Stamina;

        Current = CurrentMax;
        MinRegenDelay = minRegenDelay;
        MaxRegenDelay = maxRegenDelay;
        RegenRate = rate;

        DefaultRegenDelay = MinRegenDelay;
        DefaultRegenRate = RegenRate;

        RegenTimer = 0;
        
    }

    public void Reduce(float amount)
    {
        if (Current <=0) return;

        Current -= amount;  
        RegenTimer = 0;

        NotifyChange(Current);
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

        NotifyChange(Current);
    }

    public void ResetStamina()
    {
        Current = CurrentMax;
        NotifyChange(Current);
    }

    public void SetRegenDelay(float delay) => MinRegenDelay = delay;

    public void SetRegenRate(float rate)=> RegenRate = rate;

    public void ResetCurrentRegenDelay() => MinRegenDelay = DefaultRegenDelay;

    public void ResetRegenRate()=> RegenRate = DefaultRegenRate;


   
}
