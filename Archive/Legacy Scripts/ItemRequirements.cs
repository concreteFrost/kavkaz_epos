public enum StatType
{
    Health = 0,
    Stamina = 1,
    Knowledge = 2,
    Speed = 3,
}

[System.Serializable]   
public class ItemRequirements : IItemRequirements
{
    public StatType statType;
    public int minRequired;
    public bool CanUse(int level)=> level >= minRequired;    
}
