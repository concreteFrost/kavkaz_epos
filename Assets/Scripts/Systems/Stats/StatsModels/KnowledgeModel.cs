[System.Serializable]
public class KnowledgeModel : LevelStatModel
{
    protected override float PerLevelBonus => 20f;
    protected override float DiminishFactor => 0.9f;

    public KnowledgeModel(float baseKnowledge)
    {
        BaseInit(baseKnowledge); 
    }

   
}
