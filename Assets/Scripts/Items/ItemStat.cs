public enum ItemStatFormatType
{
    flat = 0,
    percent = 1
}

public struct ItemStat
{
    public string key;
    public float value;
    public ItemStatFormatType formatType;


    public ItemStat(string key, float value, ItemStatFormatType formatType)
    {
        this.key = key;
        this.value = value;
        this.formatType = formatType;   
    }
}


