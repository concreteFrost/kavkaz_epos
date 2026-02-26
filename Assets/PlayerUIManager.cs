
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private PlayerInventoryUI inventoryUI;
    [SerializeField] private PlayerStatsUI playerStatsUI;
    [SerializeField] private LockOnTargetUI lockOnTargetUI;
    [SerializeField] private PlayerQuickSlotsUI quickSlotsUI;
    [SerializeField] private PlayerInventoryContextMenuUI inventoryContextMenuUI;

    public void Init(CharacterStatsController stats, CharacterSpellInventory spellInventory, HumanoidCombatInventory combatInventory, PlayerTargetLock targetLock)
    {
        playerStatsUI.Init(stats: stats);
        quickSlotsUI.Init(combatInventory: combatInventory, spellInventory:spellInventory);
        inventoryUI.Init(spellInventory:spellInventory,contextMenu:inventoryContextMenuUI);
        lockOnTargetUI.Init(targetLock: targetLock);    
    }

    private void Start()
    {
        CloseAllPanels();
    }

    public void ToggleInventoryPanel(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = CursorLockMode.None;

        ToggleInGamePanels(false);

        inventoryUI.ToggleInventory(isVisible);
        inventoryUI.GetSection(InventorySection.Magic);
    }

    public void CloseAllPanels()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ToggleInGamePanels(true);

        inventoryContextMenuUI.HideContextMenu();
        inventoryUI.ToggleInventory(false);
        
    }

    private void ToggleInGamePanels(bool isVisible)
    {
        playerStatsUI.SetStatsVisible(isVisible);  
        quickSlotsUI.SetSlotsVisible(isVisible);

    }

   
}
