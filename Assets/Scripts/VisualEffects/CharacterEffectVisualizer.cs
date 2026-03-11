using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterEffectVisualizer : MonoBehaviour
{
    [SerializeField] private VisualStatusEffectDataBaseSO effectDataBaseSO;
    [SerializeField] private Transform effectPosition;

    private Dictionary<string, GameObject> activeInstances = new();

    public void ShowEffect(string id)
    {
        if (activeInstances.TryGetValue(id, out var existingInstance))
        {
            if (existingInstance.activeInHierarchy) return;

            StartCoroutine(GraduateFXToggle(existingInstance, true));
            return;
        }

        var effectData = effectDataBaseSO.sideEffects.Find(e => e.effectData.id == id );
        if (effectData == null || effectData.prefab == null)
            return;

        var instance = Instantiate(effectData.prefab, effectPosition);
        activeInstances[id] = instance;

        StartCoroutine(GraduateFXToggle(instance, true));
    }

    public void HideEffect(string id)
    {
        if (activeInstances.TryGetValue(id, out var instance))
        {
            StartCoroutine(GraduateFXToggle(instance, false));
        }
    }

    public void HideAllEffects()
    {
        foreach (var instance in activeInstances.Values)
        {
            StartCoroutine(GraduateFXToggle(instance, false));
        }
    }

    private IEnumerator GraduateFXToggle(GameObject obj, bool isOn)
    {
        var particles = obj.GetComponentsInChildren<ParticleSystem>(true);

        if (isOn)
        {
            obj.SetActive(true);

            foreach (var ps in particles)
                ps.Play(true);

            yield break;
        }

        foreach (var ps in particles)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        // ∆дЄм, пока все дочерние системы завершатс€
        bool alive;
        do
        {
            alive = false;
            foreach (var ps in particles)
            {
                if (ps.IsAlive(true))
                {
                    alive = true;
                    break;
                }
            }

            yield return null;

        } while (alive);

        obj.SetActive(false);
    }
}