using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "Item", menuName = ScriptablePaths.ITEMS_PATH + "/Item")]
public abstract class ItemSO : ScriptableObject
{
    [HideInInspector] public string id;
    public string itemName;
    public Sprite itemImage;

    [TextArea]
    public string itemDescription;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
