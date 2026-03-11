using System.Collections.Generic;


[System.Serializable]
public class StatusEffectData
{
    public StatusEffectSO statusEffectSO;
    public float effectAmount;
    public float duration;
   
    public List<StatusEffectSO> effectsToCancel = new List<StatusEffectSO>();

}

