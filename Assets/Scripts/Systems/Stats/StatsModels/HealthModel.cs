using System;

[System.Serializable]
public class HealthModel : ResourceStatModel
{

    protected override float PerLevelBonus => 15f;
    protected override float DiminishFactor => 0.85f;

    public HealthModel(float baseValue, float minRegenDelay = 0, float maxRegenDelay = 0, float rate = 0)
    {    
        statType = global::StatType.Health;
        modelType = global::ModifiedModelType.Health;   
        BaseInit(baseValue, minRegenDelay, maxRegenDelay, rate);
    }

  
}
