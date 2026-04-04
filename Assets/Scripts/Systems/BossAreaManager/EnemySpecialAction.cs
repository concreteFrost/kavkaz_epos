using System;
using System.Collections;
using UnityEngine;

// Спецприем босса
[Serializable]
public class EnemySpecialAction
{
    public string debugName;
    public BaseAISpecialMove specialMove; // состояние для выполнения

    public float minCooldown;
    public float maxCooldown;

    public bool isProcessing = false;   // выполняется ли сейчас
    public float currentCooldown;       // текущий кулдаун
    [NonSerialized] public float lastTimeExecuted = 0f; // время последнего использования

    // Инициализация (выбор случайного кулдауна)
    public void Init()
    {
        currentCooldown = UnityEngine.Random.Range(minCooldown, maxCooldown);
        lastTimeExecuted = Time.time;
    }

    // Готов ли спецприем к исполнению
    public bool IsReady() => Time.time - lastTimeExecuted >= currentCooldown;

    // Помечаем как выполненный и обновляем кулдаун
    public void MarkExecuted()
    {
        lastTimeExecuted = Time.time;
        currentCooldown = UnityEngine.Random.Range(minCooldown, maxCooldown);
        isProcessing = false;
    }

    // Запуск спецприема через корутину
    public IEnumerator StartExecuteCoroutine(EnemyBrain brain, Action onEnd)
    {
        isProcessing = true;

        // Принудительно меняем состояние мозга на спецприем
        brain.ForceChangeState(specialMove);

        // Ждем завершения выполнения спецприема
        while (!specialMove.isFinished) yield return null;

        onEnd?.Invoke();
        MarkExecuted();
    }
}