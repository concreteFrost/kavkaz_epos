using System.Collections.Generic;


public enum StatModifierOperation
{
    Increase = 0,
    Decrease = 1
}

[System.Serializable]   
public class StatusEffectData
{
    public StatModifierOperation operationType;
    public StatusEffectType effectType;
    public StatType statToAffect;

    public List<StatusEffectType> effectsToCancel = new List<StatusEffectType>();

    public float statsAffectMultiplier;

    public void Apply(CharacterStatsController stats)
    {

        var affectedStat = stats.GetStatModel(statToAffect);

        if (affectedStat == null) return;

        if (operationType == StatModifierOperation.Increase)
            affectedStat.IncreaseCurrent(statsAffectMultiplier);

        else
            affectedStat.ReduceCurrent(statsAffectMultiplier);
    }

}