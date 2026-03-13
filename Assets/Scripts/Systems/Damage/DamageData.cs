using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public float damageMultiplier;
    public BalanceDamageType balanceDamageType;
    public float impactForce;
    public StatusEffectData statusEffectData;

    [SerializeField] private float finalDamage;

    public float GetFinalDamage() => finalDamage;

    public void SetFinalDamage(float baseDamage, float strength)
    {
        strength = Mathf.Max(1f, strength);
        finalDamage = baseDamage * (strength / 100f) * (1 + damageMultiplier);

       
    }

}

