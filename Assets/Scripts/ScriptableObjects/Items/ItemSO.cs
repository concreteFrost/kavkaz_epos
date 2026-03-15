using System;
using UnityEngine;

//[CreateAssetMenu(fileName = "Item", menuName = ScriptablePaths.ITEMS_PATH + "/Item")]
public abstract class ItemSO : ScriptableObject
{
    [HideInInspector] public string id;

    [Tooltip("Имя предмета")]
    public string itemName;

    [Tooltip("Иконка отображаемая в инвентаре")]
    public Sprite itemImage;

    [Header("Description")]
    [TextArea]
    [Tooltip("Описание предмета")]
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
