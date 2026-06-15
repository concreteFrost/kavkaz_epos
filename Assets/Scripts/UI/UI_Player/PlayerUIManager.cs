using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    #region Serialized Fields

    [Header("Main Panels")]
    [SerializeField] private PlayerInventoryUI inventoryUI;
    [SerializeField] private ItemDescriptionPanelUI itemDescriptionPanel;
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private PlayerQuickSlotsUI quickSlotsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI;
    [SerializeField] private PlayerStatusEffectsUI statusEffectsUI;

    [Header("Inventory")]
    [SerializeField] private PlayerInventoryContextMenuUI inventoryContextMenuUI;

    [Header("Progression")]
    [SerializeField] private PlayerPointsControllerUI pointsControllerUI;
    [SerializeField] private PlayerLevelControllerUI levelControllerUI;

    [Header("Menu")]
    [SerializeField] private PlayerMenuOptionsUI menuOptionsUI;

    [Header("Interaction")]
    [SerializeField] private PlayerInteractionUI interactionUI;

    [Header("Money")]
    [SerializeField] private PlayerMoneyUI moneyUI;

    #endregion

    #region Initialization

    public void Init(
        PlayerInteractionController interactionController,
        CharacterStatsController stats,
        CharacterStatsModifier statsModifier,
        CharacterWeaponInventory weaponInventory,
        CharacterSpellInventory spellInventory,
        PlayerConsumableInventory consumableInventory,
        HumanoidWeaponSetter weaponSetter,
        PlayerTargetLock targetLock,
        CharacterLevelController levelController,
        CharacterConsumeController consumeController,
        PlayerMoneyManager moneyManager
        )
    {
        InitCorePanels(stats:stats,
            statsModifier:statsModifier,
            spellInventory:spellInventory,
            weaponInventory:weaponInventory, 
            consumableInventory: consumableInventory,
            weaponSetter: weaponSetter,
            targetLock: targetLock, 
            consumeController:consumeController
            );
        
        pointsControllerUI.Init(levelController);
        levelControllerUI.Init(levelController);
        menuOptionsUI.Init(levelControllerUI);
        interactionUI.Init(interactionController);
        moneyUI.Init(moneyManager);


    }

    private void InitCorePanels(
        CharacterStatsController stats,
        CharacterStatsModifier statsModifier,
        CharacterWeaponInventory weaponInventory,
        CharacterSpellInventory spellInventory,
        PlayerConsumableInventory consumableInventory,
        HumanoidWeaponSetter weaponSetter,
        CharacterConsumeController consumeController,
        PlayerTargetLock targetLock)
    {
        playerStatsUI.Init(stats);
        quickSlotsUI.Init(
            weaponSetter:weaponSetter,
            spellInventory:spellInventory,
            consumableInventory:consumableInventory,
            statsController:stats);
        inventoryUI.Init(descriptionPanel: itemDescriptionPanel,
            weaponInventory: weaponInventory,
            spellInventory: spellInventory,
            consumableInventory: consumableInventory,
            contextMenu: inventoryContextMenuUI,
            statsController: stats);
           
        inventoryContextMenuUI.Init(consumableController:consumeController);
        lockOnTargetUI.Init(targetLock);
        statusEffectsUI.Init(statsModifier: statsModifier);
    }


    private void OnEnable()
    {
        GameStateManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
    }

    #endregion

    #region Game State Handling

    private void OnGameStateChanged(GameState state)
    {
        ToggleInGamePanels(state == GameState.Game);
        SetCursorState(state != GameState.Game);    

        if (state == GameState.Inventory)
        {
            OpenInventoryPanel();
            return;
        }
        if(state == GameState.Menu)
        {
            OpenMenuPanel();
            return;
        }

        CloseAllPanels();
    }

    private void OpenInventoryPanel()
    {

        inventoryUI.ToggleInventory(true);
        inventoryUI.GetSection(InventorySection.Weapons);
    }

    private void OpenMenuPanel()
    {
        menuOptionsUI.ToggleMenuOptions(true);
    }


    private void CloseAllPanels()
    {
        
        inventoryContextMenuUI.HideContextMenu(false);
        inventoryUI.ToggleInventory(false);
        menuOptionsUI.ToggleMenuOptions(false);
        levelControllerUI.ToggleLevelControllerPanel(false);
      
    }

    #endregion

    #region Public Controls

    public void ToggleInventoryPanel(bool isVisible)
    {
        inventoryUI.ToggleInventory(isVisible);
    }

    public void ToggleMenuOptions(bool isVisible)
    {
        menuOptionsUI.ToggleMenuOptions(isVisible);
    }

    public void HideAdditionalPanels(GameState state)
    {
        if (state == GameState.Inventory)
        {
            inventoryContextMenuUI.HideContextMenu(true);
            return;
        }
 
    }

    public void ReadSliderValue(float value)
    {
        if (inventoryUI.IsOpened())
        {
            inventoryUI.RedSliderValue(value);
            return;
        }
        if (levelControllerUI.IsOpened())
        {
            levelControllerUI.HandleStatChange(value);
            return;
        }
        
    }

    public void ChangeInventorySection(int value)
    {
        if (!inventoryUI.IsOpened()) return;

        inventoryUI.SwitchSectionOnInputChange(value);  
    }

    #endregion

    #region Helpers

    private void ToggleInGamePanels(bool isVisible)
    {
        playerStatsUI.SetStatsVisible(isVisible);
        quickSlotsUI.SetPanelVisible(isVisible);
        moneyUI.ToggleWrapper(isVisible);
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