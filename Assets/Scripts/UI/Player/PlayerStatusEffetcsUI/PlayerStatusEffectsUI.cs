using System.Collections.Generic;
using UnityEngine;
public class PlayerStatusEffectsUI : MonoBehaviour
{
    CharacterStatsModifier statsModifier;

    public GameObject sliderPrefab;
    public Transform slidersParent;
    public VisualStatusEffectDataBaseSO statusEffectsDB;
    Dictionary<string, StatusEffectSliderUI> sliderMap = new Dictionary<string, StatusEffectSliderUI>();

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
            sliderMap[e.effectData.id] = data;
            data.SetEffect(e.effectData);
            data.Hide();
                    
        }
      
    }

    void OnEffectAdded(string id, float amount)
    {
        if (!sliderMap.TryGetValue(id, out var sliderUI)) return;
        sliderUI.Show();
       
    }

    void OnEffectUpdated(string id, float amount)
    {
        if (!sliderMap.TryGetValue(id, out var sliderUI)) return;

        sliderUI.Tick(amount);
    }

    void OnEffectRemoved(string id)
    {
        if (!sliderMap.TryGetValue(id, out var sliderUI)) return;

        sliderUI.Hide();    
    }

}
