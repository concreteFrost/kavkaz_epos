using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonfirePanelUI : MonoBehaviour
{
    private BonfireManager bonfireManager;

    [Header("Wrappers")]
    [SerializeField] private GameObject mainWrapper;
    [SerializeField] private GameObject bonfirePanel;
    [SerializeField] private GameObject travelWrapper;

    [Header("Button prefab")]
    [SerializeField] GameObject travelButtonPrefab;

    [Header("Action Buttons")]
    [SerializeField] Button travelSectionButton;
    [SerializeField] Button closeButton;

    private List<Button> travelButtonsPool = new List<Button>();

    public GameObject activePanel;

    public void Init()
    {
        bonfireManager = FindAnyObjectByType<BonfireManager>();
    }

    private void OnEnable()
    {
        travelSectionButton.onClick.AddListener(() => HideTravelPanel(false));
        closeButton.onClick.AddListener(CloseBonfireMenu);
    }
    private void OnDisable()
    {
        travelSectionButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();   
    }

    public void ToggleMainPanel(bool isActive)
    {
        mainWrapper.SetActive(isActive);
        bonfirePanel.SetActive(isActive);

        activePanel = bonfirePanel;
        ClampActivePanel(activePanel);


    }
    public void HideTravelPanel(bool hide)
    {
        if (hide)
        {
            travelWrapper.SetActive(false);

            activePanel = bonfirePanel;
            ClampActivePanel(activePanel);
            return;
        }

        travelWrapper.SetActive(true);
        SetupTravelActions();

        activePanel = travelWrapper;    
        ClampActivePanel(activePanel);

      
    }

    public void HideAllPanels()
    {
        HideAllButtons();
        travelWrapper.SetActive(false);
        mainWrapper.SetActive(false);
    }

    public void CloseBonfireMenu()
    {
        HideAllPanels();
        GameStateManager.GameStateChanged?.Invoke(GameState.Game);
    }

    private void SetupTravelActions()
    {

        var allActiveBonfires = bonfireManager.GetDiscoveredBonfires();

        foreach (var bonfire in allActiveBonfires)
        {
            var bonfireButton = GetTravelButtonFromPool();
            string id = bonfire.id; // копируем значение в локальную переменную
            bonfireButton.onClick.AddListener(() => bonfireManager.FastTravel(id));
            var btnText = bonfireButton.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = bonfire.GetBonfireName();
        }
    }

    private Button GetTravelButtonFromPool()
    {
        foreach(var btn in travelButtonsPool)
        {
            if (!btn.gameObject.activeSelf)
            {
                btn.gameObject.SetActive(true);
                return btn;
            }
        }

        var go = Instantiate(travelButtonPrefab, travelWrapper.transform);

        var newButton = go.GetComponent<Button>();
        travelButtonsPool.Add(newButton);   
        return newButton;
    }

    private void HideAllButtons()
    {
        foreach (var btn in travelButtonsPool)
        {
            btn.onClick.RemoveAllListeners();
            btn.gameObject.SetActive(false);    
        }
    }

    private void ClampActivePanel(GameObject activePanel)
    {
        var selectables = activePanel.GetComponentsInChildren<Selectable>().ToList();

        UINavigationUtils.ClampVerticalNavigation(selectables);

        if(selectables.Count > 0) 
        StartCoroutine(UINavigationUtils.SelectWithDelay(selectables[0].gameObject));
    }


   
}
