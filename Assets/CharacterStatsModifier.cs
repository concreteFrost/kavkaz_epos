using System.Collections.Generic;
using UnityEngine;


public class CharacterStatsModifier : MonoBehaviour
{

    public List<ActiveSideEffect> activeEffects = new List<ActiveSideEffect>();
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
            effect.timeRemaining -= Time.deltaTime;
            effect.ApplySideEffect(Time.deltaTime, statsController);

            if (effect.timeRemaining <= 0)
            {
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void ClearAllStats()
    {
        activeEffects.Clear();  
    }

  

    public void AddSideEffect(SideEffectData data)
    {
        var match = activeEffects.Find((x) => x.type == data.sideEffect);

        if(match != null)
        {
            Debug.Log("this side effect is already active");
            return;
        }

        ActiveSideEffect newEffect = new ActiveSideEffect(data.sideEffect, data.duration,data.effectMultiplier);
        visualizer.ShowEffect(newEffect.type);

        activeEffects.Add(newEffect);
    }

    
}
