using System.Collections.Generic;
using UnityEngine;
public class PlayerStatusEffectsUI : MonoBehaviour
{
    CharacterStatsModifier statsModifier;

    public GameObject sliderPrefab;
    public Transform slidersParent;

    Dictionary<string, StatusEffectSliderUI> sliderMap = new Dictionary<string, StatusEffectSliderUI>();

    public void Init(CharacterStatsModifier statsModifier)
    {
        this.statsModifier = statsModifier;

        statsModifier.EffectAdded += OnEffectAdded;
        statsModifier.EffectRemoved += OnEffectRemoved;
        statsModifier.EffectUpdated += OnEffectUpdated;

    }

    private void OnDisable()
    {
        statsModifier.EffectAdded -= OnEffectAdded;
        statsModifier.EffectRemoved -= OnEffectRemoved;
        statsModifier.EffectUpdated -= OnEffectUpdated;
    }



    void OnEffectAdded(ContinuousStatusEffectSO effect, float amount)
    {
        if (!sliderMap.TryGetValue(effect.id, out var sliderUI))
        {
            GameObject go = Instantiate(sliderPrefab, slidersParent);

            sliderUI = go.GetComponent<StatusEffectSliderUI>();
            sliderMap[effect.id] = sliderUI;
            sliderUI.SetEffect(effect);
            //sliderUI.Hide();
        }
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
