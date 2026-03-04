[System.Serializable]
public class KnowledgeModel : BaseStatModel
{
    protected override float PerLevelBonus => 20f;
    protected override float DiminishFactor => 0.9f;

    public KnowledgeModel(float baseKnowledge)
    {
        baseValue = baseKnowledge;  
    }

}
