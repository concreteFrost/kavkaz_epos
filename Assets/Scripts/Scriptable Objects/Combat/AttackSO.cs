using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum FromHand
{
    left = 0,
    right = 1,  
}

[System.Serializable]
public class WeaponAttack
{
    public DamageData damageData;

    [Header("Дистанция запуска ИИ")]
    [Min(0f)]
    [Tooltip("Минимальная дистанция начала атаки. Для обычной серии рекомендуется ноль; для прыжка позволяет запретить запуск вплотную.")]
    public float minAttackDistance;

    [FormerlySerializedAs("minDistance")]
    [Min(0f)]
    [Tooltip("Максимальная дистанция начала атаки. Для комбо проверяется только первый удар; продолжение серии от расстояния не зависит.")]
    public float maxAttackDistance = 1.3f;

    /// <summary>Проверяет расстояние до цели перед запуском атаки ИИ.</summary>
    public bool IsInRange(float distance) =>
        maxAttackDistance > 0f &&
        distance >= minAttackDistance && distance <= maxAttackDistance;

    public float staminaPenalty = 1f;

    public FromHand fromHand = FromHand.right;

    public AnimationInfoSO animationInfo;

    public FromHand SourceHand() => fromHand;

    public float invincibleStartFrame = 0f;
    public float invincibleEndFrame = 0f;    

}



[CreateAssetMenu(fileName = "AttackSet", menuName = ScriptablePaths.WEAPON_ATTACK_PATH + "/AttackSet")]
public class AttackSO : ScriptableObject
{
    public List<WeaponAttack> attackList;

    public WeaponAttack powerAttack;
}
