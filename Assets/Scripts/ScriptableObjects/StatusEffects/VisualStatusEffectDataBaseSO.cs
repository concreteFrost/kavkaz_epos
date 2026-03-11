using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   
public class VisualStatusEffect
{
    public GameObject prefab;
    public StatusEffectSO effectData;
   
}

[CreateAssetMenu(fileName = "Side Effects Data", menuName = ScriptablePaths.VFX_SIDE_FX_PATH)]
public class VisualStatusEffectDataBaseSO : ScriptableObject
{
    public List<VisualStatusEffect> sideEffects;
}
