using UnityEngine;
using System.Collections.Generic;

public class CharacterEffectVisualizer : MonoBehaviour
{
  
    public List<EffectVFX> effectsPrefabs;
    [SerializeField] private Transform effectPosition;


    public void ShowEffect(SideEffectType type)
    {
        var effect = effectsPrefabs.Find(e => e.type == type);
        if (effect == null) return;

        if (effect.instance == null)
        {
            effect.instance = Instantiate(effect.vfxPrefab, effectPosition);
        }
        effect.instance.SetActive(true);
    }

    public void HideEffect(SideEffectType type)
    {
        var effect = effectsPrefabs.Find(e => e.type == type);
        if (effect?.instance != null)
        {
            effect.instance.SetActive(false);
        }
    }
}