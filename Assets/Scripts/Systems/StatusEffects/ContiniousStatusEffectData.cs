

[System.Serializable]
public class ContiniousStatusEffectData : StatusEffectData
{

    public float duration;

    public float accumulationIncreaseMultiplier;
    public float accumulationDecreaseMultiplier;

    public void ApplyContinuous(CharacterStatsController stats, float deltaTime)
    {
        var affectedStat = stats.GetStatModel(statToAffect);
        if (affectedStat == null) return;

        float amount = statsAffectMultiplier * deltaTime;

        if (operationType == StatModifierOperation.Increase)
            affectedStat.IncreaseCurrent(amount);
        else
            affectedStat.ReduceCurrent(amount);
    }

}
