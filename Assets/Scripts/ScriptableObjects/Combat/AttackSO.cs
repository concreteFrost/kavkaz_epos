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

    public float GetFinalHealthDamage(float baseDamage)=> baseDamage + (baseDamage * damageData.healthDamageMultiplier);

    public FromHand SourceHand() => fromHand;

    public float GetClipDuration(Animator animator)
    {
        return animationInfo.clip.length / Mathf.Max(animator.speed, 0.0001f);
    }
}



[CreateAssetMenu(fileName = "AttackSet", menuName = ScriptablePaths.WEAPON_ATTACK_PATH + "/AttackSet")]
public class AttackSO : ScriptableObject
{
    public List<WeaponAttack> attackList;

    public WeaponAttack powerAttack;
 
}
