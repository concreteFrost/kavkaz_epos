
using UnityEngine;

[System.Serializable]   
public class ItemRequirements : IItemRequirements
{
    [Tooltip("“ип характеристики, котора€ провер€етс€ (например Strength, Intelligence и т.п.).")]
    public StatType statType;

    [Tooltip("ћинимальное значение характеристики, необходимое дл€ использовани€.")]
    public int minRequired=1;
    public bool CanUse(int level)=> level >= minRequired;    
}
