using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsModifier : MonoBehaviour
{

    public List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();
    CharacterStatsController statsController;
    CharacterEffectVisualizer visualizer;

    public Action<StatusEffectType, float> EffectAdded;
    public Action<StatusEffectType, float> EffectUpdated;
    public Action<StatusEffectType> EffectRemoved;

    public void Init(CharacterStatsController statsController, CharacterEffectVisualizer visualizer)
    {
        this.statsController = statsController;
        this.visualizer = visualizer;
    }

    private void Update()
    {
        if (activeEffects.Count == 0) return;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];

            EffectUpdated?.Invoke(effect.data.effectType, effect.accumulation);
            if (effect.Tick(Time.deltaTime, statsController))
            {
                CancelStatEffect(effect);
               
                continue;
            }

            if (effect.isActive)
                visualizer.ShowEffect(effect.data.effectType);
        }
    }

    public void ClearAllStats()
    {
        visualizer.HideAllEffects();
        activeEffects.Clear();

    }

    private void CancelStatEffect(StatusEffectInstance instance)
    {
        activeEffects.Remove(instance);
        visualizer.HideEffect(instance.data.effectType);
        EffectRemoved?.Invoke(instance.data.effectType);
    }

    public void TryCancelStatusEffects(List<StatusEffectType> types)
    {
        if (types.Count == 0) return;


        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {

            var effect = activeEffects[i];

            if (types.Contains(effect.data.effectType))
            {
                CancelStatEffect(effect);
            }
        }
    }

    public void GetAndApplyStatusEffect(StatusEffectSO effect,float amount)
    {
        if (effect is ContinuousStatusEffectSO) { AddContiniousSideEffect(effect as ContinuousStatusEffectSO,amount); return; }
        if (effect is StatusEffectSO) { ApplyInstantSideEffect(effect as StatusEffectSO,amount); return; }
    }

    private void ApplyInstantSideEffect(StatusEffectSO data, float amount)
    {
        TryCancelStatusEffects(data.effectsToCancel);
        data.Apply(statsController,amount);
    }

    private void AddContiniousSideEffect(ContinuousStatusEffectSO data, float amount)
    {
        TryCancelStatusEffects(data.effectsToCancel);

        var match = activeEffects.Find(x => x.data.effectType == data.effectType);

        if (match != null)
        {

            if (!match.isActive)
            {
                match.IncreaseDuration();
            }

            return;
        }

        var newEffect = new StatusEffectInstance(data,amount);

        activeEffects.Add(newEffect);
        EffectAdded?.Invoke(data.effectType, 0);
    }


}