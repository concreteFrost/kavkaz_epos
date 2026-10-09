using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Keeps the currently selected UI element visible inside this ScrollRect.
/// Attach once to the same GameObject as the ScrollRect.
/// </summary>
[RequireComponent(typeof(ScrollRect))]
public sealed class ScrollRectSelectionFollower : MonoBehaviour
{
    private ScrollRect scrollRect;
    private GameObject previousSelection;
  

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    private void OnEnable()
    {
        // Recheck the current selection when the panel containing this list is reopened.
        previousSelection = null;
    }

    private void LateUpdate()
    {
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem == null)
            return;

        GameObject selectedObject = eventSystem.currentSelectedGameObject;
        if (selectedObject == previousSelection)
            return;

        previousSelection = selectedObject;

       

        if (selectedObject == null || scrollRect.content == null)
            return;

        RectTransform selectedRect = selectedObject.GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport != null
            ? scrollRect.viewport
            : scrollRect.transform as RectTransform;

        if (selectedRect == null || viewport == null || !selectedRect.IsChildOf(scrollRect.content))
            return;

        // The selected control can be nested inside a card. Scroll the whole list item,
        // not just its button, so the card itself remains fully visible.
        RectTransform listItem = selectedRect;
        while (listItem.parent != scrollRect.content)
        {
            listItem = listItem.parent as RectTransform;
            if (listItem == null)
                return;
        }
      

        Canvas.ForceUpdateCanvases();

        Bounds selectedBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(viewport, listItem);
        Rect viewportRect = viewport.rect;
        float verticalOffset = 0f;

        if (selectedBounds.max.y > viewportRect.yMax)
            verticalOffset = viewportRect.yMax - selectedBounds.max.y;
        else if (selectedBounds.min.y < viewportRect.yMin)
            verticalOffset = viewportRect.yMin - selectedBounds.min.y;

        if (Mathf.Approximately(verticalOffset, 0f))
            return;

        float scrollableHeight = scrollRect.content.rect.height - viewport.rect.height;
        if (scrollableHeight <= 0f)
            return;

        scrollRect.StopMovement();
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(
            scrollRect.verticalNormalizedPosition - verticalOffset / scrollableHeight);
    }
}
