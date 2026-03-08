using UnityEngine;

[CreateAssetMenu(fileName = "Continuous Status Effect", menuName = ScriptablePaths.STATUS_FX_PATH + "/Continuous Status Effect")]
public class ContinuousStatusEffectSO : StatusEffectSO
{
    public float duration;

    public float accumulationIncreaseMultiplier;
    public float accumulationDecreaseMultiplier;

    public void ApplyContinuous(CharacterStatsController stats, float deltaTime)
    {
        var affectedStat = stats.GetModifiedStat(statToAffect);
        if (affectedStat == null) return;

        float amount = statsAffectMultiplier * deltaTime;

        if (operationType == StatModifierOperation.Increase)
            affectedStat.IncreaseCurrent(amount);
        else
            affectedStat.ReduceCurrent(amount);
    }
}

