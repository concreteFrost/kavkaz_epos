[System.Serializable]
public class StrengthModel: ResourceStatModel
{
    protected override float PerLevelBonus => 12f;
    protected override float DiminishFactor => 0.93f;

    public StrengthModel(float baseStrength, float minRegenDelay = 0, float maxRegenDelay = 0, float rate = 0)
    {
        statType =global::StatType.Strength;    
        modelType = global::ModifiedModelType.Strength;

        BaseInit(baseStrength,minRegenDelay,maxRegenDelay,rate);
    }
}
