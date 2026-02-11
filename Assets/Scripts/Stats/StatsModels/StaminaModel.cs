using System;
using UnityEngine;

[System.Serializable]   
public class StaminaModel
{
    public float Current;
    public float Max { get; }
    public float RegenTimer { get; private set; }
    public float MinRegenDelay { get; private set; }
    public float MaxRegenDelay { get; private set; }
    public float RegenRate { get; private set; }

    private float DefaultRegenDelay;
    private float DefaultRegenRate;

    public event Action<float> Changed;


    public StaminaModel(float max, float minRegenDelay, float maxRegenDelay, float rate)
	{
        Max = max;
        Current = max;
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
        
        Changed?.Invoke(Current);   
    }

    public void Regen()
    {
        if (Current >= Max)
        {
            Current = Max;
            return;
        }

        float regenDelay =
            Current <= 0.1f ? MaxRegenDelay : MinRegenDelay;

        RegenTimer += Time.deltaTime;

        if (RegenTimer < regenDelay)
            return;

        Current += RegenRate * Time.deltaTime;
        Current = Mathf.Clamp(Current, 0, Max);

        Changed?.Invoke(Current);
    }

    public void ResetStamina()
    {
        Current = Max;
        Changed?.Invoke(Current);   
    }

    public void SetRegenDelay(float delay) => MinRegenDelay = delay;

    public void SetRegenRate(float rate)=> RegenRate = rate;

    public void ResetCurrentRegenDelay() => MinRegenDelay = DefaultRegenDelay;

    public void ResetRegenRate()=> RegenRate = DefaultRegenRate;
}
