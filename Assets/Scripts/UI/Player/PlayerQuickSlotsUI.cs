using UnityEngine;

public class PlayerQuickSlotsUI : MonoBehaviour
{
    [Header("Wrapper")]
    [SerializeField] private GameObject wrapper;

    [Header("Equipment Slots")]
    [SerializeField] private QuickSlotBreakableUI weaponItem;
    [SerializeField] private QuickSlotBreakableUI shieldItem;

    [Header("Quick Access Slots")]
    [SerializeField] private QuickSlotItemUI spellItem;
    [SerializeField] private QuickSlotItemUI resourceItem;

    private HumanoidCombatInventory combatInventory;
    private QuickAccessInventory spellInventory;

    /// <summary>
    /// Инициализация UI
    /// </summary>
    /// <param name="combatInventory">Экипировка игрока</param>
    /// <param name="spellInventory">Инвентарь быстрых слотов (магия/ресурсы)</param>
    public void Init(HumanoidCombatInventory combatInventory, QuickAccessInventory spellInventory)
    {
        this.combatInventory = combatInventory;
        this.spellInventory = spellInventory;

        // Подписка на обновления текущего элемента инвентаря
        spellInventory.OnCurrentItemChanged += OnSpellUpdated;

        // Подписка на обновления экипировки
        combatInventory.WeaponDataUpdated += OnWeaponUpdated;
        combatInventory.ShieldUpdated += OnShieldUpdated;

        // Инициализация текущих данных
        OnSpellUpdated(spellInventory.CurrentItem);
        combatInventory.GetCurrentWeaponData();
        combatInventory.GetCurrentShieldData();
    }

    private void OnDisable()
    {
        if (spellInventory != null)
            spellInventory.OnCurrentItemChanged -= OnSpellUpdated;

        if (combatInventory != null)
        {
            combatInventory.WeaponDataUpdated -= OnWeaponUpdated;
            combatInventory.ShieldUpdated -= OnShieldUpdated;
        }
    }

    /// <summary>
    /// Показать/скрыть все быстрые слоты
    /// </summary>
    public void SetSlotsVisible(bool visible) => wrapper.SetActive(visible);

    #region Quick Slots Handlers

    private void OnSpellUpdated(ItemData currentItem)
    {
        if (currentItem != null)
        {
            
            spellItem.UpdateImageDate(currentItem);
        }
        else
        {
            spellItem.RemoveData();
        }
    }

    #endregion

    #region Equipment Handlers

    private void OnWeaponUpdated(ItemSO data, IBreakable weapon)
    {
        if (weapon == combatInventory.DefaultWeapon || weapon == null)
        {
            weaponItem.RemoveData();
            return;
        }

        weaponItem.UpdateWeaponData(data, weapon);
    }

    private void OnShieldUpdated(ItemSO data, IBreakable shield)
    {
        if (shield == null)
        {
            shieldItem.RemoveData();
            return;
        }

        shieldItem.UpdateWeaponData(data, shield);
    }

    #endregion
}