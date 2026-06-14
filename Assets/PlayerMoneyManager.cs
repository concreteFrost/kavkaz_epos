using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    [SerializeField] private float currentBalance;
    public float CurrentBalance => currentBalance;

    public Action<float> NotifyBalance;

    private readonly Queue<int> balanceQueue = new();
    private Coroutine balanceCoroutine;

    public void AddMoney(int amount)
    {
        if (amount == 0)
            return;

        balanceQueue.Enqueue(amount);

        if (balanceCoroutine == null)
            balanceCoroutine = StartCoroutine(ProcessQueue());
    }

    public bool TrySpendMoney(int amount)
    {
        if (amount <= 0)
            return false;

        if (currentBalance < amount)
            return false;

        AddMoney(-amount);
        return true;
    }

    public bool HasEnoughMoney(int amount)
    {
        return currentBalance >= amount;
    }

    public void LoadData(float amount)
    {
        currentBalance = amount;
        NotifyBalance?.Invoke(currentBalance);
    }

    private IEnumerator ProcessQueue()
    {
        while (balanceQueue.Count > 0)
        {
            int amount = balanceQueue.Dequeue(); //значение нового баланса хранится в очереди

            float startBalance = currentBalance;
            float targetBalance = Mathf.Max(0, startBalance + amount);

            while (!Mathf.Approximately(currentBalance, targetBalance))
            {
                currentBalance = Mathf.MoveTowards(
                    currentBalance,
                    targetBalance,
                    50f * Time.deltaTime);

                NotifyBalance?.Invoke(currentBalance); //формат валют

                yield return null;
            }

            currentBalance = targetBalance;
            NotifyBalance?.Invoke(currentBalance);
        }

        balanceCoroutine = null;
    }
}