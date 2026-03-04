using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelControllerUI : MonoBehaviour
{
    private CharacterLevelController levelController;

    [SerializeField] GameObject wrapper;
    [SerializeField] Transform statPanelsWrapper;
    [SerializeField] GameObject upgraderPanelPrefab;

    Dictionary<StatUpgraderPanelUI, StatType> statDatas = new Dictionary<StatUpgraderPanelUI, StatType>();

    private List<Selectable> selectablePanels = new List<Selectable>();

    private Selectable currentSelected;
    public void Init(CharacterLevelController levelController)
    {
        this.levelController = levelController;

        InstantiateStats();
        ToggleLevelControllerPanel(false);    

    }

    private void InstantiateStats()
    {
        StatType[] statsToUpgrade =
    {
        StatType.Health,
        StatType.Stamina,
        StatType.Knowledge
    };

        foreach (StatType statType in statsToUpgrade)
        {
            GameObject go = Instantiate(upgraderPanelPrefab, statPanelsWrapper);

            var panel = go.GetComponent<StatUpgraderPanelUI>();

            panel.Init(levelController, statType);
            panel.AddButtonListeners();
            panel.UpdatePointsUI(); 

            statDatas.Add(panel, statType);
            selectablePanels.Add(go.GetComponent<Selectable>());    
        }
    }

    public void ToggleLevelControllerPanel(bool isVisible)
    {
        // ≈сли скрываем панель, автоматически примен€ем очки
        if (!isVisible)
        {
            Upgrade();
        }

        wrapper.SetActive(isVisible);

        // ќбновл€ем UI независимо от видимости
        foreach (StatUpgraderPanelUI panel in statDatas.Keys)
        {
            panel.UpdatePointsUI();
        }

        HighlightFirst();
    }

    public void Upgrade()
    {
        foreach (var panel in statDatas.Keys)
        {
            int points = panel.ConsumeAccumulatedPoints();

            while (points > 0)
            {

                levelController.SpendPoint(statDatas[panel]);
                points--;
            }
        }
       
    }

    #region Gamepad PanelsInteraction
    private void HighlightFirst()
    {
        UINavigationUtils.ClampVerticalNavigation(selectablePanels);
        if (selectablePanels.Count == 0) return;

        currentSelected = selectablePanels[0];
        StartCoroutine(UINavigationUtils.SelectWithDelay(currentSelected.gameObject));
    }

    public void HandleStatChange(float val)
    {
        if (currentSelected == null) return;

        var panel = currentSelected.GetComponent<StatUpgraderPanelUI>();
        if (panel == null) return;

        if (val > 0)
            panel.AccumulatePoint();
        else if (val < 0)
            panel.RemoveAccumulatedPoint();
    }

    #endregion


}
