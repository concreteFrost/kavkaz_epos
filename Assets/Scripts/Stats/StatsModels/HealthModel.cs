using System;

public class HealthModel
{
    public float Current { get; set; }
    public float Max { get; }

    public event Action<float> Changed;
    public event Action Depleted;

    public HealthModel(float max)
    {
        Max = max;
        Current = max;
    }

    public void Damage(float amount)
    {
        if (Current <= 0) return;

        Current -= amount;
        if (Current < 0) Current = 0;

        Changed?.Invoke(Current);

        if (Current == 0)
            Depleted?.Invoke();
    }

    public void ResetHealth(float max)
    {
        Current = max;
        Changed?.Invoke(Current);   
    }
}
