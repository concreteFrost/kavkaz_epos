using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class BossPhaseState
{
    public string debugName;

    [Range(0f, 1f)]
    public float minActivationValue = 0f;

    [Range(0f, 1f)]
    public float maxActivationValue = 1f;

    public List<EnemySpecialAction> specialActions = new List<EnemySpecialAction>();

    public bool IsInPhase(float normalizedHealth)
    {
        return normalizedHealth <= maxActivationValue && normalizedHealth > minActivationValue;
    }
}
