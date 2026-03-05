using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatInfoPanelUI : MonoBehaviour, ISelectHandler
{
    CharacterLevelController levelController;
    
    [SerializeField] TextMeshProUGUI statNameText;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] Button updateStatBtn;
    [SerializeField] Button downgradeStatBtn;

    PlayerLevelControllerUI uiController;
   
    StatType statType;

    private int accumulatedPoints;

    public void Init(CharacterLevelController levelController, StatType statType , PlayerLevelControllerUI uiController)
    {
        this.levelController = levelController;
        this.statType = statType;
        this.uiController = uiController;   

        statNameText.text = statType.ToString();
        accumulatedPoints = 0;
        currentLevelText.text = "";
 
    }

    public void AddButtonListeners()
    {
        updateStatBtn.onClick.RemoveAllListeners();
        updateStatBtn.onClick.AddListener(AccumulatePoint);

        downgradeStatBtn.onClick.RemoveAllListeners();
        downgradeStatBtn.onClick.AddListener(RemoveAccumulatedPoint);
    }

    public void UpdatePointsUI()
    {
        var statsController = levelController.GetStatsController();
        var total = statsController.GetCurrentStatLevel(statType) + accumulatedPoints;
        currentLevelText.text = statsController.GetStatModel(statType).CurrentMax.ToString();   
        pointsText.text = total.ToString();
    }

    public bool HasAccumulatedPoints()
    {
        return accumulatedPoints > 0;
    }

    public void AccumulatePoint()
    {
        if (levelController.GetUnspentPoints() == 0) return;

        accumulatedPoints++;
        levelController.ReserveSpendPoint();
        UpdatePointsUI();
    }

    public void RemoveAccumulatedPoint()
    {
        
        if (accumulatedPoints <= 0)
            return; // нечего возвращать

        accumulatedPoints--;
        levelController.RefundSpendPoint();
        UpdatePointsUI();
    }


    public int ConsumeAccumulatedPoints()
    {
        int points = accumulatedPoints;
        accumulatedPoints = 0;
       
        return points;
    }


    public void OnSelect(BaseEventData eventData)
    {
        uiController.CurrentSelected = this.GetComponent<Selectable>(); 
    }
}
