using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{

    public IDamagable target;
    public Transform source;

    public ProjectileDirection direction;
    public IAttackSource attackSource;

    public ProjectileSO projectileSO;
    //public DamageData finalDamageData;

    //public float GetFinalDamage(float baseDamage, float strength)
    //{
    //    strength = Mathf.Max(1f, strength);

    //    float finalDamage = baseDamage * (strength / 100f) * (1 + finalDamageData.healthDamageMultiplier);

    //    Debug.Log(finalDamage);
    //    return finalDamage;
    //}

}
