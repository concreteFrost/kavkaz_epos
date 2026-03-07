using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   
public class StatusEffectSO
{
    public GameObject prefab;
    public StatusEffectType type;
}

[CreateAssetMenu(fileName = "Side Effects Data", menuName = ScriptablePaths.VFX_SIDE_FX_PATH)]
public class StatusEffectDataBaseSO : ScriptableObject
{
    public List<StatusEffectSO> sideEffects;
}
