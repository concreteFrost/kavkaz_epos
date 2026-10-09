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


    public abstract bool CanUse();
    public abstract bool CanEquip();
    public abstract bool CanAddToSlot();
    public abstract bool CanRemoveFromSlot();

    public abstract bool CanDestroy();
}


