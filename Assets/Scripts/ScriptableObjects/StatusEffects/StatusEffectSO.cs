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


[System.Serializable]
public enum BuffColorType
{
    Fire=0,
    Health=1,
    Poison=2,
    Blood=3,
    Stamina=4,
    Strength=5
}


[CreateAssetMenu(fileName = "Instant Status Effect", menuName = ScriptablePaths.STATUS_FX_PATH + "/Instant Status Effect")]
public class StatusEffectSO : ScriptableObject
{
    public string id;
    public Sprite effectImage;

    public BuffColorType colorType;
    public Color effectColor => buffColors[(int)colorType];

    public StatusEffectType effectType;
    public ModifiedModelType statToAffect;

    public StatModifierOperation statOperation;
    public OperationType operationType;



    private static readonly Color[] buffColors = new Color[]
   {
        new Color(1f, 0.45f, 0f),   // Fire
        new Color(0.2f, 1f, 0.2f),  // Health
        new Color(0.6f, 0f, 0.8f),  // Poison
        new Color(1f, 0f, 0f),      // Blood
        new Color(0.1f, 0.6f, 1f),  // Stamina
        new Color(1,1,1,1)          // Strength
   };

    public virtual void Apply(CharacterStatsController stats, float amount=1)
    {
        var affectedStat = stats.GetModifiedStat(statToAffect);

        if (affectedStat == null) return;

        if (statOperation == StatModifierOperation.ChangeCurrent)
            affectedStat.ChangeCurrent(amount,operationType);
   
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



