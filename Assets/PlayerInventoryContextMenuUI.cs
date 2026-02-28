using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryContextMenuUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] Button addToSlotBtn;
    [SerializeField] Button removeFromSlotBtn;

    RectTransform _rectTransform;
    ItemData currentItem;

    public List<Selectable> allSelectables = new List<Selectable>();
    public Action<ItemData> OnContextMenuClosed;

    PlayerControls pl;
    public void Init(QuickAccessInventory quickAccessInventory)
    {
        allSelectables.AddRange(wrapper.GetComponentsInChildren<Button>());
        _rectTransform = wrapper.GetComponent<RectTransform>();
        SetupAction(addToSlotBtn, () => quickAccessInventory.AddToQuickAccess(currentItem));
        SetupAction(removeFromSlotBtn, () => quickAccessInventory.RemoveFromQuickAccess(currentItem));
        
    }


    void SetupAction(Button btn, Action action)
    {
        btn.onClick.RemoveAllListeners();

        btn.onClick.AddListener(() =>
        {
            action?.Invoke();
            HideContextMenu();
         
        });
    }

    public void HideContextMenu()
    {
        OnContextMenuClosed?.Invoke(currentItem);
        wrapper.SetActive(false);
        currentItem = null;
       
    }

    public void ShowContextMenu(ItemData data, Vector2 position)
    {
        if (!WillShowContextMenu(data))
        {
            HideContextMenu();
            return;
        }

        currentItem = data;

        wrapper.SetActive(true);

        position.y -= 90;
        _rectTransform.localPosition = position;

        UINavigationUtils.ClampVerticalNavigation(allSelectables);
        StartCoroutine(UINavigationUtils.SelectWithDelay(allSelectables[0].gameObject));
    }


    private bool WillShowContextMenu(ItemData data)
    {
        if (currentItem != null)
        {
            if (currentItem.itemSO.id == data.itemSO.id)
            {
                return false; // не показываем меню, если тот же предмет
            }
        }
        return true;
    }
}
