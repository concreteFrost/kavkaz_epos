using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelControllerUI : MonoBehaviour
{
    private CharacterLevelController levelController;

    [SerializeField] GameObject wrapper;
    [SerializeField] Transform statPanelsWrapper;
    [SerializeField] GameObject statInfoPanelPrefab;
    [SerializeField] GameObject upgraderPanelPrefab;

    public Action<int> PointsUpdated;
    public Action<int> LevelUpdated;

    Dictionary<StatInfoPanelUI, StatType> statDatas = new Dictionary<StatInfoPanelUI, StatType>();
    StatsUpgraderPanelUI upgraderPanelUI;

    private List<Selectable> selectablePanels = new List<Selectable>();

    private Selectable currentSelected;

    public Selectable CurrentSelected { get => currentSelected; set => currentSelected = value; }

    public bool IsOpened() => wrapper.activeInHierarchy;
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
            GameObject go = Instantiate(statInfoPanelPrefab, statPanelsWrapper);

            var panel = go.GetComponent<StatInfoPanelUI>();

            panel.Init(levelController, statType, this);
            panel.AddButtonListeners();
            panel.UpdatePointsUI();

            statDatas.Add(panel, statType);
            selectablePanels.Add(go.GetComponent<Selectable>());
        }

        GameObject upgraderPrefab = Instantiate(upgraderPanelPrefab, statPanelsWrapper);
        
        StatsUpgraderPanelUI panelUI = upgraderPrefab.GetComponent<StatsUpgraderPanelUI>();
        panelUI.Init(this);
        
        upgraderPanelUI = panelUI;
        selectablePanels.Add(panelUI.GetComponent<Selectable>());
    }

    public void ToggleLevelControllerPanel(bool isVisible)
    {

        wrapper.SetActive(isVisible);

        if (!isVisible) return;
        // Обновляем UI независимо от видимости
        foreach (StatInfoPanelUI panel in statDatas.Keys)
        {
            panel.UpdatePointsUI();
        }

        upgraderPanelUI.UpdateCharacterLevelText(levelController.levelData.currentCharacterLevel);
        upgraderPanelUI.UpdateUnspentPointsText(levelController.levelData.unspentPoints);

        UpdateUpgradeButtonState();

        HighlightFirst();

    }

    public void UpdateUpgradeButtonState()
    {
        bool hasPoints = false;

        foreach (var panel in statDatas.Keys)
        {
            if (panel.HasAccumulatedPoints())
            {
                hasPoints = true;
                break;
            }
        }

        upgraderPanelUI.SetButtonActive(hasPoints);
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

        UpdateUpgradeButtonState();
        
        foreach (StatInfoPanelUI panel in statDatas.Keys)
        {
            panel.UpdatePointsUI();
        }

    }

    #region Gamepad Panels Interaction
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

        if (currentSelected.TryGetComponent<StatInfoPanelUI>(out StatInfoPanelUI panel))
        {
            if (panel == null)
            {
                return;
            }

            if (val > 0)
            {
                panel.AccumulatePoint();
            }
               

            else if (val < 0)
                panel.RemoveAccumulatedPoint();

            upgraderPanelUI.UpdateUnspentPointsText(levelController.levelData.unspentPoints);
            
        }
    }

    #endregion


}
