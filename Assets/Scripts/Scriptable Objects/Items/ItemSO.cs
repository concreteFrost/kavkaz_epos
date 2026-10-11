using UnityEngine;


//[CreateAssetMenu(fileName = "Item", menuName = ScriptablePaths.ITEMS_PATH + "/Item")]
public abstract class ItemSO : WithIdSO
{
   
    public abstract bool IsStackable();

    [Tooltip("Имя предмета")]
    public string itemName;

    [Tooltip("Иконка отображаемая в инвентаре")]
    public Sprite itemImage;

    [Header("Description")]
    [TextArea]
    [Tooltip("Описание предмета")]
    public string itemDescription;


    private const int SellingPricePercent = 35;
    private const int PercentDivisor = 100;

    [Header("Стоимость")]
    [Min(0)]
    [Tooltip("Базовая цена покупки предмета без улучшений, в монетах.")]
    public int price;

    [SerializeField]
    [Tooltip("Цена продажи: 35% базовой цены с округлением вниз. Пересчитывается автоматически.")]
    private int sellingPrice;

    public int Price => price;
    public int SellingPrice => sellingPrice;

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        price = Mathf.Max(0, price);
        sellingPrice = (int)((long)price * SellingPricePercent / PercentDivisor);
    }
#endif

    public abstract bool CanUse();
    public abstract bool CanEquip();
    public abstract bool CanAddToSlot();
    public abstract bool CanRemoveFromSlot();

    public abstract bool CanDestroy();
}


