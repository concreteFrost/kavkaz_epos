using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgraderPanelUI : MonoBehaviour
{
    CharacterLevelController levelController;
    
    [SerializeField] TextMeshProUGUI statNameText;
    [SerializeField] TextMeshProUGUI pointsText;
    [SerializeField] TextMeshProUGUI currentLevelText;
    [SerializeField] Button updateStatBtn;
    [SerializeField] Button downgradeStatBtn;
   
    StatType statType;

    private int accumulatedPoints;

    public void Init(CharacterLevelController levelController, StatType statType)
    {
        this.levelController = levelController;
        this.statType = statType;

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

    public void AccumulatePoint()
    {
        if (levelController.GetUnspentPoints() == 0) return;

        accumulatedPoints++;
        levelController.ReserveSpendPoint();
        UpdatePointsUI();
    }

    public void RemoveAccumulatedPoint()
    {
        Debug.Log("removing unspent point");
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
        UpdatePointsUI();
        return points;
    }


}
