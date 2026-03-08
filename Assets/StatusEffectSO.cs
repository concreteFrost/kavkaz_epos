using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instant Status Effect", menuName = ScriptablePaths.STATUS_FX_PATH + "/Instant Status Effect")]
public class StatusEffectSO : ScriptableObject
{
    public StatModifierOperation operationType;
    public StatusEffectType effectType;
    public ModifiedModelType statToAffect;

    public List<StatusEffectType> effectsToCancel = new List<StatusEffectType>();

    public float statsAffectMultiplier;

    public void Apply(CharacterStatsController stats)
    {
        var affectedStat = stats.GetModifiedStat(statToAffect);

        if (affectedStat == null) return;

        if (operationType == StatModifierOperation.Increase)
            affectedStat.IncreaseCurrent(statsAffectMultiplier);
        else
            affectedStat.ReduceCurrent(statsAffectMultiplier);
    }
}

