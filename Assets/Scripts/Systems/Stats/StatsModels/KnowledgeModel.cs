[System.Serializable]
public class KnowledgeModel : ResourceStatModel
{

    public KnowledgeModel(float baseKnowledge, float minRegenDelay = 0, float maxRegenDelay = 0, float rate = 0)
    {
        statType = global::StatType.Knowledge;  
        modelType = global::ModifiedModelType.Knowledge;
        BaseInit(baseKnowledge, minRegenDelay, maxRegenDelay, rate); 

    }

   
}
