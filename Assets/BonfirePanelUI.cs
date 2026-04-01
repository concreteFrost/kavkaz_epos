using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonfirePanelUI : MonoBehaviour
{

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
        HideTravelPanel(true);



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
        var bonfireManager = FindAnyObjectByType<BonfireManager>();

        // Сначала скрываем все кнопки из пула
        foreach (var btn in travelButtonsPool)
        {
            btn.onClick.RemoveAllListeners();
            btn.gameObject.SetActive(false);
        }

        // Создаем/переиспользуем кнопки для текущего списка bonfires
        foreach (var bonfire in bonfireManager.GetDiscoveredBonfires())
        {
            var bonfireButton = GetTravelButtonFromPool();

            string id = bonfire.id;
            bonfireButton.onClick.AddListener(() => bonfireManager.FastTravel(id));

            var btnText = bonfireButton.GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = bonfire.GetBonfireName();
        }
    }

    private Button GetTravelButtonFromPool()
    {
        // Находим первую свободную кнопку
        var btn = travelButtonsPool.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
        if (btn != null)
        {
            btn.gameObject.SetActive(true);
            return btn;
        }

        // Если нет свободной, создаём новую
        var go = Instantiate(travelButtonPrefab, travelWrapper.transform);
        btn = go.GetComponent<Button>();
        travelButtonsPool.Add(btn);
        return btn;
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
