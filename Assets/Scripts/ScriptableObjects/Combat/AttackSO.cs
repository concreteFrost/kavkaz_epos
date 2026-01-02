using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public float healthDamageMultiplier;
    public float balanceDamageMultiplier;
    public float staminaPenalty;

    public AnimationInfoSO animationInfo;

    public float GetFinalHealthDamage(float baseDamage)
    {
        return baseDamage + (baseDamage * healthDamageMultiplier);
    }

    public float GetFinalBalanceDamage()
    {
        return balanceDamageMultiplier;
    }
}


[CreateAssetMenu(fileName = "AttackSet", menuName = "Scriptable Systems/Combat/AttackSet")]
public class AttackSO : ScriptableObject
{
    public List<Attack> attackList;
 
}
