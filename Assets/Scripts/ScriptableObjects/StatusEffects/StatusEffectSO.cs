using System;
using UnityEngine;

public enum StatModifierOperation
{
    ChangeMax = 0,
    ChangeCurrent = 1,
    ChangeRegenRate = 2,
}

public enum OperationType
{
    Positive = 0,
    Negative = 1
}


[CreateAssetMenu(fileName = "Instant Status Effect", menuName = ScriptablePaths.STATUS_FX_PATH + "/Instant Status Effect")]
public class StatusEffectSO : ScriptableObject
{
    [Tooltip("Уникальный идентификатор эффекта. Генерируется автоматически если поле пустое.")]
    public string id;

    [Tooltip("Иконка эффекта, отображаемая в UI (баффы/дебаффы).")]
    public Sprite effectImage;

    [Tooltip("Тип статус-эффекта ")]
    public StatusEffectType effectType;

    [Tooltip("Характеристика персонажа, на которую влияет эффект")]
    public ModifiedModelType statToAffect;

    [Tooltip("Какой параметр характеристики изменяется: текущее значение, максимум или скорость регенерации.")]
    public StatModifierOperation statOperation;

    [Tooltip("Определяет является ли эффект положительным (бафф) или отрицательным (дебафф).")]
    public OperationType operationType;


    private static readonly Color32[] buffColors = new Color32[]
   {
        new Color32(255,255,255,255),   // Fire
        new Color32(230, 32, 18, 255),
};
    public Color32 effectColor => buffColors[(int)operationType];

    public virtual void Apply(CharacterStatsController stats, float amount = 1)
    {
        var affectedStat = stats.GetModifiedStat(statToAffect);

        if (affectedStat == null) return;

        if (statOperation == StatModifierOperation.ChangeCurrent)
            affectedStat.ChangeCurrent(amount, operationType);

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
        }
    }

#endif


}

