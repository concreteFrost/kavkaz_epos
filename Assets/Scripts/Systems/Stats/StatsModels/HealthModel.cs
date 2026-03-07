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

        baseValue = baseHealth; 
        CurrentMax = baseValue;
        Current = CurrentMax;
    }

    public override void ReduceCurrent(float amount)
    {
        if (Current <= 0) return;

        Current -= amount;
        if (Current < 0) Current = 0;

        NotifyChange(Current);

        if (Current == 0)
            Depleted?.Invoke();
  
    }

    public override void IncreaseCurrent(float amount)
    {
        if(Current >= CurrentMax) return;   
        Current += amount;

        NotifyChange(Current);  
    } 
    public void ResetHealth()
    {
        Current = CurrentMax;
        NotifyChange(Current);   
    }
}
