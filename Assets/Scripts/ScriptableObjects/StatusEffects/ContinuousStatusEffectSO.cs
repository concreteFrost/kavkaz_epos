using UnityEngine;

[CreateAssetMenu(fileName = "Continuous Status Effect", menuName = ScriptablePaths.STATUS_FX_PATH + "/Continuous Status Effect")]
public class ContinuousStatusEffectSO : StatusEffectSO
{

    [HideInInspector] public float accumulationIncreaseMultiplier = 0.12f;
    [HideInInspector] public float accumulationDecreaseMultiplier = 0.1f;

    public GameObject visualAppearance;
    public bool useAccumulation = false;

    public void OnApply(CharacterStatsController stats, float amount)
    {
        var stat = stats.GetModifiedStat(statToAffect);
        if (stat == null) return;

        switch (statOperation)
        {
            case StatModifierOperation.ChangeMax:
                stat.ChangeMax(id, amount, operationType);
                break;

            case StatModifierOperation.ChangeRegenRate:
                stat.ChangeRegenRate(id, amount, operationType);
                break;
        }
    }

    public virtual void OnRemove(CharacterStatsController stats)
    {

        var stat = stats.GetModifiedStat(statToAffect);
        if (stat == null) return;

        switch (statOperation)
        {
            case StatModifierOperation.ChangeMax:
                stat.ResetMax(id);
                break;
            case StatModifierOperation.ChangeRegenRate:
                stat.ResetRegenRate(id);
                break;
        }
    }

    public virtual void Tick(CharacterStatsController stats, float amount)
    {
        var stat = stats.GetModifiedStat(statToAffect);
        if (stat == null) return;

        switch (statOperation)
        {
            case StatModifierOperation.ChangeCurrent:
                stat.ChangeCurrent(amount, operationType);
                break;

        }
    }

}

