using System.Collections.Generic;
using UnityEngine;
public class PlayerStatusEffectsUI : MonoBehaviour
{
    CharacterStatsModifier statsModifier;

    public GameObject sliderPrefab;
    public Transform slidersParent;
    public VisualStatusEffectDataBaseSO statusEffectsDB;
    Dictionary<StatusEffectType, StatusEffectSliderUI> sliderMap = new Dictionary<StatusEffectType, StatusEffectSliderUI>();

    public void Init(CharacterStatsModifier statsModifier)
    {
        this.statsModifier = statsModifier;

        statsModifier.EffectAdded += OnEffectAdded;
        statsModifier.EffectRemoved += OnEffectRemoved;
        statsModifier.EffectUpdated += OnEffectUpdated;

        InitPrefabs();

    }

    private void OnDisable()
    {
        statsModifier.EffectAdded -= OnEffectAdded;
        statsModifier.EffectRemoved -= OnEffectRemoved;
        statsModifier.EffectUpdated -= OnEffectUpdated;
    }

    private void InitPrefabs()
    {
        foreach(var e in statusEffectsDB.sideEffects)
        {
            GameObject go = Instantiate(sliderPrefab, slidersParent);

            StatusEffectSliderUI data = go.GetComponent<StatusEffectSliderUI>();
            sliderMap[e.type] = data;
            data.SetEffect(e.type, e.effectImage,e.effectColor);
            data.Hide();
            
            
        }
      
    }

    void OnEffectAdded(StatusEffectType type, float amount)
    {
        if (!sliderMap.TryGetValue(type, out var sliderUI)) return;

        sliderUI.Show();
       
    }

    void OnEffectUpdated(StatusEffectType type, float amount)
    {
        if (!sliderMap.TryGetValue(type, out var sliderUI)) return;

        sliderUI.Tick(amount);
    }

    void OnEffectRemoved(StatusEffectType type)
    {
        if (!sliderMap.TryGetValue(type, out var sliderUI)) return;

        sliderUI.Hide();    
    }

}
