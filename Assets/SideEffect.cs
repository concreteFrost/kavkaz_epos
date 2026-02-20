using UnityEngine;

public enum SideEffectType
{
    Burn = 0,
    Poison = 1,
}

[System.Serializable]
public class ActiveSideEffect
{
    public SideEffectType type;
    public float timeRemaining;
    public float multiplier;
    public float accumulation; // дл€ Poison-style эффектов

    public ActiveSideEffect(SideEffectType type, float duration, float multiplier)
    {
        this.type = type;
        this.timeRemaining = duration;
        this.multiplier = multiplier;
        this.accumulation = 0f;
    }

    public void ApplySideEffect(float dt, CharacterStatsController statsController)
    {
        switch (type)
        {
            case SideEffectType.Burn:
                statsController.Health.Reduce(multiplier * dt);
                break;

            case SideEffectType.Poison:
                accumulation += multiplier * dt;
                if (accumulation >= 1f)
                {
                    statsController.Health.Reduce(1f);
                    accumulation = 0f;
                }
                break;

                // другие эффекты через enum добавл€ютс€ сюда
        }
    }
}


[System.Serializable]
public struct SideEffectData
{
    public SideEffectType sideEffect;
    public float duration;
    public float effectMultiplier;
    //public float accumulation;  
}

[System.Serializable]
public class EffectVFX
{
    public SideEffectType type;
    public GameObject vfxPrefab;
    [HideInInspector] public GameObject instance;
}
