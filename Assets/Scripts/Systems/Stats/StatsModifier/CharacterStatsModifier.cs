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

    public void GetAndApplyStatusEffect(StatusEffectData data)
    {
        if (data == null || data.effects == null) return;

        foreach (var entry in data.effects)
        {
            TryCancelStatusEffects(entry.effectsToCancel);

            if (entry.effect is ContinuousStatusEffectSO continuous)
            {
                AddContiniousSideEffect(continuous, entry.amount, entry.duration);
            }
            else
            {
                ApplyInstantSideEffect(entry);
            }
        }
    }

    private void ApplyInstantSideEffect(StatusEffectEntry entry)
    {
        if (entry == null || entry.effect == null) return;

        // отмен€ем нужные эффекты
        TryCancelStatusEffects(entry.effectsToCancel);

        // примен€ем сам эффект
        entry.effect.Apply(statsController, entry.amount);
    }

    private void AddContiniousSideEffect(
     ContinuousStatusEffectSO effect,
     float amount,
     float duration)
    {
        var match = activeEffects.Find(x => x.data.id == effect.id);

        if (match != null)
        {
            if (!effect.useAccumulation || !match.isActive)
            {
                match.IncreaseDuration();
            }
            return;
        }

        var newEffect = new StatusEffectInstance(effect, amount, duration);

        activeEffects.Add(newEffect);

        effect.OnApply(statsController, amount);

        EffectAdded?.Invoke(effect.id, 0);
    }

}