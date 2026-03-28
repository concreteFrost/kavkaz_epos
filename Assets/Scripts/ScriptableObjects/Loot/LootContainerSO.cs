using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropableItem
{
    public ItemSO itemSO;

    [Min(1)]
    public int minQuantity;

    public int maxQuantity;

    [Range(0, 1)]
    public float dropChance;

    // Этот метод будет проверять значения прямо в инспекторе
    public void OnValidate()
    {
        // min не может быть меньше 0
        if (minQuantity < 0) minQuantity = 0;

        // min не может быть больше max
        if (minQuantity > maxQuantity) minQuantity = maxQuantity;
    }
}

[CreateAssetMenu(menuName = ScriptablePaths.LOOT_PATH + "/Loot Container", fileName ="Loot Container")]
public class LootContainerSO : ScriptableObject
{
    public string id;
    public List<DropableItem> possibleItems = new List<DropableItem>();

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        foreach (var item in possibleItems)
        {
            item.OnValidate();
        }
    }
}
