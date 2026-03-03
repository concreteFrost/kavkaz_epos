using System;

[System.Serializable]
public class HealthModel : BaseStatModel
{

    public event Action Depleted;

    protected override float PerLevelBonus => 15f;
    protected override float DiminishFactor => 0.85f;

    public HealthModel(float baseHealth)
    {

        statType = global::StatType.Health;
        Current = CurrentMax;
    }

    public void Reduce(float amount)
    {
        if (Current <= 0) return;

        Current -= amount;
        if (Current < 0) Current = 0;

        NotifyChange(Current);

        if (Current == 0)
            Depleted?.Invoke();
  
    }

    public void ResetHealth()
    {
        Current = CurrentMax;
        NotifyChange(Current);   
    }
}
