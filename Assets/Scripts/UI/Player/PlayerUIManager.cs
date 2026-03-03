
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private PlayerInventoryUI inventoryUI;
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI;
    [SerializeField] private PlayerQuickSlotsUI quickSlotsUI;
    [SerializeField] private PlayerInventoryContextMenuUI inventoryContextMenuUI;
 
    public void Init(CharacterStatsController stats, CharacterSpellInventory spellInventory, HumanoidCombatInventory combatInventory, PlayerTargetLock targetLock, CharacterLevelController levelController)
    {
        playerStatsUI.Init(stats: stats);
        quickSlotsUI.Init(combatInventory: combatInventory, statsController:stats, spellInventory:spellInventory);
        inventoryUI.Init(spellInventory:spellInventory,contextMenu:inventoryContextMenuUI, statsController:stats );
        lockOnTargetUI.Init(targetLock: targetLock);
        inventoryContextMenuUI.Init(quickAccessInventory: spellInventory);

        GameStateManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state != GameState.Inventory)
        {
            CloseAllPanels();
            return;
        }

        ToggleInventoryPanel(true);
    }

    private void CloseAllPanels()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ToggleInGamePanels(true);

        inventoryContextMenuUI.HideContextMenu();
        inventoryUI.ToggleInventory(false);

    }

    public void ToggleInventoryPanel(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = CursorLockMode.None;

        ToggleInGamePanels(!isVisible);

        inventoryUI.ToggleInventory(isVisible);
        inventoryUI.GetSection(InventorySection.Magic);
    }

  

    private void ToggleInGamePanels(bool isVisible)
    {
        playerStatsUI.SetStatsVisible(isVisible);  
        quickSlotsUI.SetPanelVisible(isVisible);

    }

    public void HideContextMenu()
    {
        inventoryContextMenuUI.HideContextMenu();
    }

    public void ReadSliderValue(float val)
    {
        inventoryUI.RedSliderValue(val);
    }
   
}

