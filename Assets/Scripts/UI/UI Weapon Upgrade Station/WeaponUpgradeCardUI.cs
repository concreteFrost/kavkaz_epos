using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTierData
{
    public WeaponData weaponData;
    public string itemName;
    public int itemLevel;
    public int nextLevel;
    public float itemDamage;
    public float nextDamage;
    public string requiredItemName;
    public int requiredItemCount;
    public int playerItemsCount;
    public float requiredMoney;
    public float playerHasMoney;

    public CardTierData(
        WeaponData weaponData,
        string itemName,
        int itemLevel,
        int nextLevel,
        float itemDamage,
        float nextDamage,
        string requiredItemName,
        int requiredItemCount,
        int playerItemsCount,
        float requiredMoney,
        float playerHasMoney)
    {
        this.weaponData = weaponData;
        this.itemName = itemName;
        this.itemLevel = itemLevel;
        this.nextLevel = nextLevel;
        this.itemDamage = itemDamage;
        this.nextDamage = nextDamage;
        this.requiredItemName = requiredItemName;
        this.requiredItemCount = requiredItemCount;
        this.playerItemsCount = playerItemsCount;
        this.requiredMoney = requiredMoney;
        this.playerHasMoney = playerHasMoney;
    }
}

public class WeaponUpgradeCardUI : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    Image rectImage;
    Color defaultRectColor;
    [SerializeField] private TextMeshProUGUI text_weaponName;
    [SerializeField] private TextMeshProUGUI text_weaponLevel;
    [SerializeField] private TextMeshProUGUI text_requiredItems;
    [SerializeField] private TextMeshProUGUI text_weaponDamage;
    [SerializeField] private TextMeshProUGUI text_requireMoney;
    [SerializeField] private Button btn_upgrade;
    [SerializeField] private Image icon;

    Outline outline;

    private WeaponData weaponData;
    private Action<WeaponData> onUpgradeRequest;

    public WeaponData Data => weaponData;

    private void Awake()
    {
        rectImage = GetComponent<Image>();
        
        outline = GetComponent<Outline>();
        outline.enabled = false;

        defaultRectColor = rectImage.color;
       
    }

    private void OnEnable()
    {
        if (btn_upgrade != null)
            btn_upgrade.onClick.AddListener(BindAction);
    }

    private void OnDisable()
    {
        if (btn_upgrade != null)
            btn_upgrade.onClick.RemoveListener(BindAction);

        rectImage.color = defaultRectColor;
 
    }

    public void SetCardData(
        CardTierData data,
        Action<WeaponData> upgradeRequest)
    {
        onUpgradeRequest = upgradeRequest;

        if (data == null)
        {
            weaponData = null;
            ClearTexts();

            if (icon != null)
                icon.sprite = null;

            ToggleButton(false);
            return;
        }

        weaponData = data.weaponData;

        text_weaponName.text = data.itemName;
        text_weaponLevel.text = $"{data.itemLevel} => {data.nextLevel}";
     
        text_weaponDamage.text = $"{data.itemDamage} => {data.nextDamage}";

        text_requiredItems.text =
            $"{data.requiredItemName}: {data.requiredItemCount} ({data.playerItemsCount})";

        text_requireMoney.text = $"{data.requiredMoney} ({data.playerHasMoney})";

        if (icon != null)
            icon.sprite = data.weaponData?.itemSO?.itemImage;

        ToggleButton(weaponData != null && upgradeRequest != null);
    }

    public void ToggleButton(bool isEnabled)
    {
        if (btn_upgrade != null)
            btn_upgrade.interactable = isEnabled;
    }

    private void BindAction()
    {
        if (weaponData != null)
            onUpgradeRequest?.Invoke(weaponData);
    }

    private void ClearTexts()
    {
        text_weaponName.text = string.Empty;
        text_weaponLevel.text = string.Empty;
        text_requiredItems.text = string.Empty;
        text_weaponDamage.text = string.Empty;
        text_requireMoney.text = string.Empty;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Color onSelectColor = new Color(defaultRectColor.r, defaultRectColor.g, defaultRectColor.b);
        onSelectColor.a = 0.5f;
        rectImage.color = onSelectColor;
        outline.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        rectImage.color = defaultRectColor;
        outline.enabled = false;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (btn_upgrade == null || !btn_upgrade.IsInteractable()) return;

        BindAction();
    }
}
