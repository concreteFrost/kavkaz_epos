using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack
{
    public float healthDamageMultiplier;
    public float balanceDamageMultiplier;
    public float staminaPenalty;

    [Header("Animation settings")]
    public AnimationClip clip;

    [Tooltip("—корость проигрывани€ анимации дл€ этой атаки")]
    public float animationSpeed = 1f;

    public float hitStartFrame;
    public float hitEndFrame;   

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
    public float maxComboDelay = 1.3f;
}
