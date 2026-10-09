[System.Serializable]
public class StrengthModel: ResourceStatModel
{


    public StrengthModel(float baseStrength, float minRegenDelay = 0, float maxRegenDelay = 0, float rate = 0)
    {
        statType =global::StatType.Strength;    
        modelType = global::ModifiedModelType.Strength;

        BaseInit(baseStrength,minRegenDelay,maxRegenDelay,rate);
    }
}
