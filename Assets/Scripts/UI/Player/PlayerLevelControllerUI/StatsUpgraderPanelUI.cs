using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatsUpgraderPanelUI : MonoBehaviour , ISelectHandler, ISubmitHandler
{
    PlayerLevelControllerUI levelController;

    [SerializeField] TextMeshProUGUI characterLevelText;
    [SerializeField] TextMeshProUGUI unspentPointsText;

    [SerializeField] Button upgradeBtn;
    
    public void Init(PlayerLevelControllerUI levelControllerUI)
    {
        this.levelController = levelControllerUI;

        upgradeBtn.onClick.RemoveAllListeners();
        upgradeBtn.onClick.AddListener(Upgrade);
    }

    public void Upgrade()=> levelController.Upgrade();    
    
    public void UpdateUnspentPointsText(int val) => unspentPointsText.text = val.ToString();   

    public void UpdateCharacterLevelText(int val) => characterLevelText.text = val.ToString();

    public void OnSelect(BaseEventData eventData)
    {
        levelController.CurrentSelected = this.GetComponent<Selectable>();
    }

    public void SetButtonActive(bool isActive) => upgradeBtn.interactable = isActive;

    public void OnSubmit(BaseEventData eventData)
    {
        Upgrade();
    }
}
