using UnityEngine;

[CreateAssetMenu(fileName = "Continuous Status Effect", menuName = ScriptablePaths.STATUS_FX_PATH + "/Continuous Status Effect")]
public class ContinuousStatusEffectSO : StatusEffectSO
{
    public float duration;

    public float accumulationIncreaseMultiplier;
    public float accumulationDecreaseMultiplier;

}

