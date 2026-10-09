using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] TextMeshProUGUI text_playerMoney;
    PlayerMoneyManager moneyManager;

    public void Init(PlayerMoneyManager moneyManager)
    {
        this.moneyManager = moneyManager;
        this.moneyManager.NotifyBalance += OnNotifyBalance;

        OnNotifyBalance(moneyManager.CurrentBalance);   

    }

    private void OnDisable()
    {
        this.moneyManager.NotifyBalance -= OnNotifyBalance;  
    }

    public void ToggleWrapper(bool isVisible) => wrapper.SetActive(isVisible);

    private void OnNotifyBalance(float amount)
    {
       
        text_playerMoney.text = amount.ToString("0.00");  
    }

    
}
