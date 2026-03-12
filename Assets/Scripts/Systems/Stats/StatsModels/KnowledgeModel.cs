[System.Serializable]
public class KnowledgeModel : ResourceStatModel
{
    protected override float PerLevelBonus => 12f;
    protected override float DiminishFactor => 0.9f;

    public KnowledgeModel(float baseKnowledge, float minRegenDelay = 0, float maxRegenDelay = 0, float rate = 0)
    {
        statType = global::StatType.Knowledge;  
        modelType = global::ModifiedModelType.Knowledge;
        BaseInit(baseKnowledge, minRegenDelay, maxRegenDelay, rate); 

    }

   
}
