using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    #region Serialized Fields

    [Header("Main Panels")]
    [SerializeField] private PlayerInventoryUI inventoryUI;
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private PlayerQuickSlotsUI quickSlotsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI;

    [Header("Inventory")]
    [SerializeField] private PlayerInventoryContextMenuUI inventoryContextMenuUI;

    [Header("Progression")]
    [SerializeField] private PlayerPointsControllerUI pointsControllerUI;
    [SerializeField] private PlayerLevelControllerUI levelControllerUI;

    [Header("Menu")]
    [SerializeField] private PlayerMenuOptionsUI menuOptionsUI;

    #endregion

    #region Initialization

    public void Init(
        CharacterStatsController stats,
        CharacterSpellInventory spellInventory,
        HumanoidCombatInventory combatInventory,
        PlayerTargetLock targetLock,
        CharacterLevelController levelController)
    {
        InitCorePanels(stats, spellInventory, combatInventory, targetLock);
        InitProgression(levelController);
        InitMenu();

        GameStateManager.GameStateChanged += OnGameStateChanged;
    }

    private void InitCorePanels(
        CharacterStatsController stats,
        CharacterSpellInventory spellInventory,
        HumanoidCombatInventory combatInventory,
        PlayerTargetLock targetLock)
    {
        playerStatsUI.Init(stats);
        quickSlotsUI.Init(combatInventory:combatInventory,spellInventory:spellInventory,statsController:stats);
        inventoryUI.Init(spellInventory, inventoryContextMenuUI, stats);
        inventoryContextMenuUI.Init(spellInventory);
        lockOnTargetUI.Init(targetLock);
    }

    private void InitProgression(CharacterLevelController levelController)
    {
        pointsControllerUI.Init(levelController);
        levelControllerUI.Init(levelController);
    }

    private void InitMenu()
    {
        menuOptionsUI.Init(levelControllerUI);
    }

    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
    }

    #endregion

    #region Game State Handling

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Inventory)
        {
            OpenInventory();
            return;
        }
        if(state == GameState.Menu)
        {
            OpenMenu();
            return;
        }

        CloseAllPanels();
    }

    private void OpenInventory()
    {
        SetCursorState(true);
        ToggleInGamePanels(false);

        inventoryUI.ToggleInventory(true);
        inventoryUI.GetSection(InventorySection.Magic);
    }

    private void OpenMenu()
    {
        SetCursorState(true);
        ToggleInGamePanels(false);

        menuOptionsUI.ToggleMenuOptions(true);
    }

    private void CloseAllPanels()
    {
        SetCursorState(false);
        ToggleInGamePanels(true);

        inventoryContextMenuUI.HideContextMenu();
        inventoryUI.ToggleInventory(false);
        menuOptionsUI.ToggleMenuOptions(false);
        levelControllerUI.ToggleLevelControllerPanel(false);
    }

    #endregion

    #region Public Controls

    public void ToggleInventoryPanel(bool isVisible)
    {
        SetCursorState(isVisible);
        ToggleInGamePanels(!isVisible);

        inventoryUI.ToggleInventory(isVisible);
    }

    public void ToggleMenuOptions(bool isVisible)
    {
        SetCursorState(isVisible);
        ToggleInGamePanels(!isVisible);

        menuOptionsUI.ToggleMenuOptions(isVisible);
    }

    public void HideContextMenu()
    {
        inventoryContextMenuUI.HideContextMenu();
    }

    public void ReadSliderValue(float value)
    {
        if (inventoryUI.gameObject.activeInHierarchy)
        {
            inventoryUI.RedSliderValue(value);
            return;
        }
        if (levelControllerUI.gameObject.activeInHierarchy)
        {
            levelControllerUI.HandleStatChange(value);
            return;
        }
        
    }

    #endregion

    #region Helpers

    private void ToggleInGamePanels(bool isVisible)
    {
        playerStatsUI.SetStatsVisible(isVisible);
        quickSlotsUI.SetPanelVisible(isVisible);
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }

    #endregion
}