using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class StatusEffectData
{
    [Tooltip("Список эффектов, которые накладываются на цель при попадании.")]
    public List<StatusEffectEntry> effects = new List<StatusEffectEntry>();

    [Tooltip("Эффекты, которые будут сняты с цели при попадании (например dispel).")]
    public List<StatusEffectSO> effectsToCancel = new List<StatusEffectSO>();
}

[System.Serializable]
public class StatusEffectEntry
{
    [Tooltip("Какой статус-эффект будет применён (яд, огонь, замедление и т.д.).")]
    public StatusEffectSO effect;

    [Tooltip("Сила эффекта. Интерпретация зависит от типа эффекта (например урон в секунду или процент замедления).")]
    public float amount;

    [Tooltip("Длительность эффекта в секундах.")]
    public float duration;


}