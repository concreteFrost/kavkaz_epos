using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public float healthDamageMultiplier;
    public BalanceDamageType balanceDamageMultiplier = BalanceDamageType.Low;
    public float staminaPenalty;

    public AnimationInfoSO animationInfo;

    public float GetFinalHealthDamage(float baseDamage)
    {
        return baseDamage + (baseDamage * healthDamageMultiplier);
    }

    public BalanceDamageType GetFinalBalanceDamage()
    {
        return balanceDamageMultiplier;
    }

    public float GetClipDuration(Animator animator)
    {
        return animationInfo.clip.length / Mathf.Max(animator.speed, 0.0001f);
    }
}


[CreateAssetMenu(fileName = "AttackSet", menuName = "Scriptable Systems/Combat/AttackSet")]
public class AttackSO : ScriptableObject
{
    public List<Attack> attackList;

    public Attack powerAttack;
 
}
