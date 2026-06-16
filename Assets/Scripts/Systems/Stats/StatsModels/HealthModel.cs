using System;

[System.Serializable]
public class HealthModel : ResourceStatModel
{


    public HealthModel(float baseValue, float minRegenDelay = 0, float maxRegenDelay = 0, float rate = 0)
    {    
        statType = global::StatType.Health;
        modelType = global::ModifiedModelType.Health;   
        BaseInit(baseValue, minRegenDelay, maxRegenDelay, rate);
    }

  
}
