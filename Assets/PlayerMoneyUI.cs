using TMPro;
using UnityEngine;

public class PlayerMoneyUI : MonoBehaviour
{
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

    private void OnNotifyBalance(float amount)
    {
        text_playerMoney.text = amount.ToString();  
    }
}
