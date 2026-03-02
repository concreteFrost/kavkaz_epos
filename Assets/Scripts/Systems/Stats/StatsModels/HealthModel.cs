using System;

[System.Serializable]
public class HealthModel : BaseStatModel
{


    public event Action Depleted;

    protected override float PerLevelBonus => 20f;
    protected override float DiminishFactor => 0.9f;

    public HealthModel(float baseHealth, int level)
    {

        statType = global::StatType.Health;
        base.level = level;   
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
