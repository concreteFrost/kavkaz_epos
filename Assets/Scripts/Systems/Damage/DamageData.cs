using UnityEngine;

[System.Serializable]
public class DamageData
{
    public float healthDamageMultiplier;
    public BalanceDamageType balanceDamageType;
    public float impactForce;
    public DamageStatusEffectData statusEffectData;

}

[System.Serializable]
public class DamageStatusEffectData
{
    public StatusEffectSO statusEffectSO;
    public float healthDamageMultiplier;
}

