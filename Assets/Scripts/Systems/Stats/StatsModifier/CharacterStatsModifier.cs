using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsModifier : MonoBehaviour
{

    public List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();
    CharacterStatsController statsController;
    CharacterEffectVisualizer visualizer;

    public Action<string, float> EffectAdded;
    public Action<string, float> EffectUpdated;
    public Action<string> EffectRemoved;

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

            EffectUpdated?.Invoke(effect.data.id, effect.Progress);

            if (effect.Tick(Time.deltaTime, statsController))
            {
                CancelStatEffect(effect);

                continue;
            }

            if (effect.isActive)
                visualizer.ShowEffect(effect.data.id);
        }
    }

    public void ClearAllStats()
    {
        visualizer.HideAllEffects();
        activeEffects.Clear();

    }

    private void CancelStatEffect(StatusEffectInstance instance)
    {
        instance.data.OnRemove(statsController);

        activeEffects.Remove(instance);
        visualizer.HideEffect(instance.data.id);
        EffectRemoved?.Invoke(instance.data.id);
    }

    public void TryCancelStatusEffects(List<StatusEffectSO> types)
    {
        if (types == null || types.Count == 0) return;

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];

            // провер€ем, есть ли id эффекта в списке effectsToCancel этого эффекта
            foreach (var cancelEffect in types)
            {
                if (effect.data.id == cancelEffect.id)
                {
                    CancelStatEffect(effect);
                    break; // раз нашли Ч дальше не ищем
                }
            }
        }
    }

    public void GetAndApplyStatusEffect(StatusEffectData effectData)
    {
        if (effectData.statusEffectSO is ContinuousStatusEffectSO) { AddContiniousSideEffect(effectData); return; }
        if (effectData.statusEffectSO is StatusEffectSO) { ApplyInstantSideEffect(effectData); return; }
    }

    private void ApplyInstantSideEffect(StatusEffectData effectData)
    {
        TryCancelStatusEffects(effectData.effectsToCancel);
        effectData.statusEffectSO.Apply(statsController, effectData.effectAmount);
    }

    private void AddContiniousSideEffect(StatusEffectData effectData)
    {
        var continuousEffect = effectData.statusEffectSO as ContinuousStatusEffectSO;
        TryCancelStatusEffects(effectData.effectsToCancel);

        var match = activeEffects.Find(x => x.data.id == effectData.statusEffectSO.id);

        if (match != null)
        {
            // если не используем накопление или эффект ещЄ не активен
            if (!continuousEffect.useAccumulation || !match.isActive)
            {
                match.IncreaseDuration();
            }
            return;
        }

        var newEffect = new StatusEffectInstance(continuousEffect,effectData.effectAmount, effectData.duration);

        activeEffects.Add(newEffect);

        continuousEffect.OnApply(statsController, effectData.effectAmount);
        EffectAdded?.Invoke(continuousEffect.id, 0);

    }


}