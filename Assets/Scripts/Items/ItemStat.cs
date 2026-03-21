public enum ItemStatFormatType
{
    flat = 0,
    percent = 1
}

public struct ItemStat
{
    public ItemStatType type;
    public float value;
    public ItemStatFormatType formatType;


    public ItemStat(ItemStatType type, float value, ItemStatFormatType formatType)
    {
        this.type = type;
        this.value = value;
        this.formatType = formatType;   
    }
}


