using System.Collections.Generic;
using UnityEngine;

public enum FromHand
{
    left = 0,
    right = 1,  
}

[System.Serializable]
public class WeaponAttack
{
    public DamageData damageData;

    public float staminaPenalty = 1f;

    public FromHand fromHand = FromHand.right;

    public AnimationInfoSO animationInfo;

    public FromHand SourceHand() => fromHand;

    //public float GetFinalDamage(float baseDamage, float strength)
    //{
    //    strength = Mathf.Max(1f, strength);

    //    float finalDamage = baseDamage * (strength / 100f) * (1 + damageData.damageMultiplier);

    //    Debug.Log(finalDamage);
    //    return finalDamage;
    //}
}



[CreateAssetMenu(fileName = "AttackSet", menuName = ScriptablePaths.WEAPON_ATTACK_PATH + "/AttackSet")]
public class AttackSO : ScriptableObject
{
    public List<WeaponAttack> attackList;

    public WeaponAttack powerAttack;

   
 
}
