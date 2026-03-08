
[System.Serializable]   
public class ItemRequirements : IItemRequirements
{
    public StatType statType;
    public int minRequired;
    public bool CanUse(int level)=> level >= minRequired;    
}
