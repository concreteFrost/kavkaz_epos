using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventoryContextMenuUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] Button addToSlotBtn;
    [SerializeField] Button removeFromSlotBtn;

    RectTransform _rectTransform;
    ItemData currentItem;

    private void Awake()
    {
        _rectTransform = wrapper.GetComponent<RectTransform>();
    }

    private void Start()
    {
        HideContextMenu();
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

        FocusOnFirstButton();
    }

    private void FocusOnFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(addToSlotBtn.gameObject);
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
