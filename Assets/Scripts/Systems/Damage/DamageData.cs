using UnityEngine;

public enum DamageSourceType
{
    None = 0,
    MeleeWeapon =1,
}

[System.Serializable]
public struct DamageData
{
    [Tooltip("Дополнительный множитель урона. Например 0.5 = +50% к итоговому урону. Применяется для оружейных атак.")]
    public float damageMultiplier;

    [Tooltip("Тип урона для системы баланса (например Slash, Fire, Magic). Используется для резистов и уязвимостей.")]
    public BalanceDamageType balanceDamageType;

    [Tooltip("Сила физического воздействия при попадании (отбрасывание, stagger и т.п.).")]
    public float impactForce;

    [Tooltip("Статус-эффекты, которые применяются при попадании.")]
    public StatusEffectData statusEffectData;

    [Tooltip("Финальный рассчитанный урон после всех множителей. Заполняется автоматически во время атаки.")]
    [HideInInspector] public float finalDamage;

    public DamageSourceType damageSourceType;

    public void SetFinalDamage(float baseDamage, float strength)
    {
        strength = Mathf.Max(1f, strength);
        finalDamage = baseDamage * (strength / 100f) * (1 + damageMultiplier);
       
    }

}

