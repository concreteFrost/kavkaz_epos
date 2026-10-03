using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    [SerializeField] private float currentBalance;
    public float CurrentBalance => currentBalance;

    private float displayBalance;

    public Action<float> NotifyBalance;

    private readonly Queue<float> balanceQueue = new();
    private Coroutine balanceCoroutine;

    private void Awake()
    {
        displayBalance = currentBalance;
    }

    public void AddMoney(float amount)
    {
        if (amount == 0)
            return;

        currentBalance = Mathf.Max(0f, currentBalance + amount);
        EnqueueBalanceAnimation(amount);
    }

    public bool TrySpendMoney(float amount)
    {
        if (amount <= 0 || currentBalance < amount)
            return false;

        currentBalance -= amount;
        EnqueueBalanceAnimation(-amount);
        return true;
    }

    public bool HasEnoughMoney(int amount)
    {
        return currentBalance >= amount;
    }

    public void LoadData(float amount)
    {
        if (balanceCoroutine != null)
        {
            StopCoroutine(balanceCoroutine);
            balanceCoroutine = null;
        }

        balanceQueue.Clear();
        currentBalance = amount;
        displayBalance = amount;
        NotifyBalance?.Invoke(displayBalance);
    }

    private void EnqueueBalanceAnimation(float amount)
    {
        balanceQueue.Enqueue(amount);

        if (balanceCoroutine == null)
            balanceCoroutine = StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        while (balanceQueue.Count > 0)
        {
            float amount = balanceQueue.Dequeue();
            float targetBalance = Mathf.Max(0f, displayBalance + amount);

            while (!Mathf.Approximately(displayBalance, targetBalance))
            {
                displayBalance = Mathf.MoveTowards(
                    displayBalance,
                    targetBalance,
                    50f * Time.deltaTime);

                NotifyBalance?.Invoke(displayBalance);
                yield return null;
            }

            displayBalance = targetBalance;
            NotifyBalance?.Invoke(displayBalance);
        }

        balanceCoroutine = null;
    }
}