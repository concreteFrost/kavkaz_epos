using System;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    [SerializeField] private int currentBalance = 0;

    public int CurrentBalance => currentBalance;

    public Action<float> NotifyBalance;

    public void AddMoney(int amount)
    {
        if (amount <= 0) return;

        currentBalance += amount;

        NotifyBalance?.Invoke(currentBalance);  
    }

    public bool TrySpendMoney(int amount)
    {
        if (amount <= 0) return false;

        if (currentBalance < amount)
            return false;

        currentBalance -= amount;
        NotifyBalance?.Invoke(currentBalance);  
        return true;
    }

    public bool HasEnoughMoney(int amount)
    {
        return currentBalance >= amount;
    }

  
}