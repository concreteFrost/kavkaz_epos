using System.Collections.Generic;


[System.Serializable]
public class StatusEffectData
{
    public List<StatusEffectEntry> effects = new List<StatusEffectEntry>();
}

[System.Serializable]
public class StatusEffectEntry
{
    public StatusEffectSO effect;

    public float amount;
    public float duration;

    public List<StatusEffectSO> effectsToCancel = new List<StatusEffectSO>();
}