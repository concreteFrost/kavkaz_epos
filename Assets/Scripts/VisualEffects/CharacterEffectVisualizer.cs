using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CharacterEffectVisualizer : MonoBehaviour
{

    [SerializeField] private Transform effectPosition;

    private Dictionary<string, GameObject> activeInstances = new();

    public void ShowEffect(ContinuousStatusEffectSO effect)
    {
        if (activeInstances.TryGetValue(effect.id, out var existingInstance))
        {
            if (existingInstance.activeInHierarchy) return;

            StartCoroutine(GraduateFXToggle(existingInstance, true));
            return;
        }

        // Создаём новый экземпляр эффекта
        var instance = Instantiate(effect.visualAppearance, effectPosition);
        activeInstances[effect.id] = instance;

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

        // Ждём, пока все дочерние системы завершатся
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