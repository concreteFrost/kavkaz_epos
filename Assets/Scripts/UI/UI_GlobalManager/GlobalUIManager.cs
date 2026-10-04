using System;
using UnityEngine;

public class GlobalUIManager : MonoBehaviour
{
    [SerializeField] private BonfirePanelUI bonfirePanelUI;
    [SerializeField] private PlayerLootPanelUI lootPanelUI;
    [SerializeField] private ScreenFaderUI screenFaderUI;

    [SerializeField] private WeaponUpgradeStationUI weaponUpgraderStationUI;
    [SerializeField] private GameMenuUI gameMenuUI;

    public static Action<bool> UiToggled;

    public static GlobalUIManager Instance;


    private void OnEnable()
    {
        GameStateManager.GameStateChanged += OnGameStateChanged;
        WeaponUpgradeStation.WeaponUpgradeStationInteracted += OnWeaponStationInteracted;
        Bonfire.BonfireInteracted += OnBonfireInteracted;
    }

    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
        WeaponUpgradeStation.WeaponUpgradeStationInteracted -= OnWeaponStationInteracted;
        Bonfire.BonfireInteracted -= OnBonfireInteracted;
    }

    private void OnWeaponStationInteracted(WeaponUpgradeStation station, IPlayerInteractor interactor)
    {
        GameStateManager.Instance.SetState(GameState.ContextMenu);
        weaponUpgraderStationUI.Show(station, interactor);
    }

    private void OnBonfireInteracted()
    {
        GameStateManager.Instance.SetState(GameState.ContextMenu);
        bonfirePanelUI.ToggleMainPanel(true);

    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        lootPanelUI.Init();
    }


    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Game || state == GameState.Transition)
        {
            CloseAllPanels();
            return;
        }
    }

    private void CloseAllPanels()
    {
        SetCursorState(false);
        bonfirePanelUI.HideAllPanels();
        weaponUpgraderStationUI.Close();
        gameMenuUI.ToggleMenuOptions(false);
    }

    public void OpenMenuPanel()
    {
        GameStateManager.Instance.SetState(GameState.ContextMenu);
        gameMenuUI.ToggleMenuOptions(true);
        UiToggled?.Invoke(true);
    }

    public void RegisterLevelControllerAction(Action openLevelController)
    {
        gameMenuUI.SetLevelControllerAction(openLevelController);
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }
}
