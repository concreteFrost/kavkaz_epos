using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SavedEffectData
{
    public string effectId;
    public float duration;
    public float amount;
    public bool isActive;
    public float defaultDuration;
    public float accumulation;
}

public class CharacterStatsModifier : MonoBehaviour
{
    public List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();
    CharacterStatsController statsController;
    CharacterEffectVisualizer visualizer;


    public Action<ContinuousStatusEffectSO, float> EffectAdded;
    public Action<string, float> EffectUpdated;
    public Action<string> EffectRemoved;

    IDamagable damagable;

    public void Init(CharacterStatsController statsController, CharacterEffectVisualizer visualizer, IDamagable damagable)
    {
        this.statsController = statsController;
        this.visualizer = visualizer;
        this.damagable = damagable;

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
                visualizer.ShowEffect(effect.data);
        }
    }

    public List<SavedEffectData> SaveEffectData()
    {
        List<SavedEffectData> savedEffectData = new List<SavedEffectData>();
        foreach (var effect in activeEffects)
        {
            SavedEffectData effectData = new SavedEffectData()
            {
                effectId = effect.data.id,
                duration = effect.duration,
                defaultDuration = effect.DefaultDuration,
                accumulation = effect.Accumulation,
                amount = effect.amount,
                isActive = effect.isActive
            };

            savedEffectData.Add(effectData);
        }

        return savedEffectData;
    }

    public void LoadEffectsData(List<SavedEffectData> effectData)
    {
        ClearAllStats();

        List<ContinuousStatusEffectSO> effectsDB = Resources.LoadAll<ContinuousStatusEffectSO>("StatusEffects").ToList();

        foreach (var saved in effectData)
        {
            var match = effectsDB.Find((x) => x.id == saved.effectId);

            if (match == null) continue;

            AddContiniousSideEffectFromSave(match, saved);

        }
    }

    public void ClearNegativeStatEffects()
    {
        if(activeEffects.Count == 0) return;

        var allNegative = activeEffects.FindAll((x) => x.data.isNegative);
        foreach (var effect in allNegative)
        {
           CancelStatEffect(effect);    
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
        if (damagable.IsDead) return;

        if (data == null || data.effects == null) return;

        TryCancelStatusEffects(data.effectsToCancel);

        foreach (var entry in data.effects)
        {


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

        EffectAdded?.Invoke(effect, 0);
    }

    private void AddContiniousSideEffectFromSave(
    ContinuousStatusEffectSO effect,
    SavedEffectData data
   )
    {
        // важно: Ќ≈ ищем match и не стакаем
        // мы восстанавливаем как есть

        var instance = StatusEffectInstance.Load(
           effect,
           data.amount,
            data.duration,
            data.defaultDuration,
            data.accumulation,
            data.isActive
        );

        activeEffects.Add(instance);
        effect.OnApply(statsController, data.amount);

        // если эффект уже активен Ч примен€ем визуал
        if (data.isActive)
            visualizer.ShowEffect(effect);

        // уведомл€ем UI (если нужно)
        EffectAdded?.Invoke(effect, instance.Progress);

    }

}