using UnityEngine;

public class PlayerQuickSlotsUI : MonoBehaviour
{
    [Header("Wrapper")]
    [SerializeField] private GameObject wrapper;

    [Header("Equipment Slots")]
    [SerializeField] private QuickSlotBreakableUI weaponItem;
    [SerializeField] private QuickSlotBreakableUI shieldItem;

    [Header("Quick Access Slots")]
    [SerializeField] private SlotItemUI spellItem;
    [SerializeField] private SlotItemUI resourceItem;
    [SerializeField] private SlotItemUI consumableItem;


    private HumanoidWeaponSetter weaponSetter;
    private QuickAccessInventory spellInventory;
    private QuickAccessInventory consumableInventory;
    private CharacterStatsController statsController;

    /// <summary>
    /// Инициализация UI
    /// </summary>
    /// <param name="weaponSetter">Экипировка игрока</param>
    /// <param name="spellInventory">Инвентарь быстрых слотов (магия/ресурсы)</param>
    public void Init(HumanoidWeaponSetter weaponSetter, QuickAccessInventory spellInventory,QuickAccessInventory consumableInventory ,CharacterStatsController statsController)
    {
        this.weaponSetter = weaponSetter;
        this.spellInventory = spellInventory;
        this.statsController = statsController;
        this.consumableInventory = consumableInventory;

        // Подписка на обновления текущего элемента инвентаря
        spellInventory.OnCurrentItemChanged += OnSpellUpdated;
        consumableInventory.OnCurrentItemChanged += OnConsumableUpdated;
        statsController.Knowledge.MaxChanged += OnMaxKnowledgeChanged;

        // Подписка на обновления экипировки
        weaponSetter.WeaponDataUpdated += OnWeaponUpdated;
        weaponSetter.ShieldUpdated += OnShieldUpdated;

        // Инициализация текущих данных
        
        weaponSetter.GetCurrentWeaponData();
        weaponSetter.GetCurrentShieldData();

    }

    //private void Start()
    //{
    //    OnSpellUpdated(spellInventory.CurrentItem);
    //}

    private void OnDisable()
    {

        spellInventory.OnCurrentItemChanged -= OnSpellUpdated;
        consumableInventory.OnCurrentItemChanged -= OnConsumableUpdated;    
        statsController.Knowledge.MaxChanged -= OnMaxKnowledgeChanged;
        weaponSetter.WeaponDataUpdated -= OnWeaponUpdated;
        weaponSetter.ShieldUpdated -= OnShieldUpdated;

    }

    /// <summary>
    /// Показать/скрыть все быстрые слоты и обновить информацию о текущих предметах
    /// </summary>
    public void SetPanelVisible(bool visible)
    {
        if (!visible) return;

        OnSpellUpdated(spellInventory.CurrentItem);
        OnConsumableUpdated(consumableInventory.CurrentItem);
    }

    #region Spell Slot

    /// <summary>
    /// Вызывается при измене уровня знаний у игрока для того чтобы убрать/показать иконку запрета на использование
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void OnMaxKnowledgeChanged(int arg1, float arg2) => OnSpellUpdated(spellInventory.CurrentItem);

    /// <summary>
    /// Отображает актуальную информацию о текущем заклинании
    /// </summary>
    /// <param name="currentItem"></param>
    private void OnSpellUpdated(ItemData currentItem)
    {
        if (currentItem != null)
        {
            spellItem.UpdateImageDate(currentItem, statsController);
        }
        else
        {
            spellItem.RemoveData();
        }
    }


    #endregion

    #region Consumable Slot

    /// <summary>
    /// Отображает актуальную информацию о текущем потребляемом ресурсе
    /// </summary>
    /// <param name="currentItem"></param>
    private void OnConsumableUpdated(ItemData currentItem)
    {
        if (currentItem != null)
        {
            
            consumableItem.UpdateImageDate(currentItem, statsController);
        }
        else
        {
            consumableItem.RemoveData();
        }
    }


    #endregion

    #region Equipment Handlers

    private void OnWeaponUpdated(ItemSO data, IBreakable weapon)
    {
        if (weapon == weaponSetter.DefaultWeapon || weapon == null)
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