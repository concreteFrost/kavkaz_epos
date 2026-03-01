using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsModifier : MonoBehaviour
{

    public List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();
    CharacterStatsController statsController;
    CharacterEffectVisualizer visualizer;

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


            if (effect.Tick(Time.deltaTime, statsController))
            {
                activeEffects.RemoveAt(i);
                visualizer.HideEffect(effect.data.type);
            }

            if (effect.isActive)
                visualizer.ShowEffect(effect.data.type);
        }
    }

    public void ClearAllStats()
    {
        visualizer.HideAllEffects();
        activeEffects.Clear();

    }

    public void AddSideEffect(StatusEffectData data)
    {
        var match = activeEffects.Find(x => x.data.type == data.type);


        if (match != null)
        {

            if (!match.isActive)
            {
                match.IncreaseDuration();

            }

            return;
        }

        var newEffect = new StatusEffectInstance(data);

        activeEffects.Add(newEffect);
    }



}