
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectAutoScroll : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    private RectTransform contentRect;


    private void Start()
    {
        //get the scroll rect components
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;

    }

    public void BringChildIntoViewVertically(RectTransform child)
    {
        //if vertical scrollbar isn't visible, return
        if (!scrollRect.verticalScrollbar.gameObject.activeSelf)
            return;

        scrollRect.content.ForceUpdateRectTransforms();
        scrollRect.viewport.ForceUpdateRectTransforms();

        // now takes scaling into account
        Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
        Vector2 childLocalPosition = child.localPosition;
        Vector2 newContentPosition = new Vector2(
            0 - ((viewportLocalPosition.x * scrollRect.viewport.localScale.x) + (childLocalPosition.x * scrollRect.content.localScale.x)),
            0 - ((viewportLocalPosition.y * scrollRect.viewport.localScale.y) + (childLocalPosition.y * scrollRect.content.localScale.y))
        );

        // clamp positions
        scrollRect.content.localPosition = newContentPosition;
        Rect contentRectInViewport = TransformRectFromTo(scrollRect.content.transform, scrollRect.viewport);
        float deltaXMin = contentRectInViewport.xMin - scrollRect.viewport.rect.xMin;
        if (deltaXMin > 0) // clamp to <= 0
            newContentPosition.x -= deltaXMin;

        float deltaXMax = contentRectInViewport.xMax - scrollRect.viewport.rect.xMax;
        if (deltaXMax < 0) // clamp to >= 0
            newContentPosition.x -= deltaXMax;

        float deltaYMin = contentRectInViewport.yMin - scrollRect.viewport.rect.yMin;
        if (deltaYMin > 0) // clamp to <= 0
            newContentPosition.y -= deltaYMin;

        float deltaYMax = contentRectInViewport.yMax - scrollRect.viewport.rect.yMax;
        if (deltaYMax < 0) // clamp to >= 0
            newContentPosition.y -= deltaYMax;

        // apply final position
        scrollRect.content.localPosition = newContentPosition;
        scrollRect.content.ForceUpdateRectTransforms();
    }

    public static Rect TransformRectFromTo(Transform from, Transform to)
    {
        RectTransform fromRectTrans = from.GetComponent<RectTransform>();
        RectTransform toRectTrans = to.GetComponent<RectTransform>();

        if (fromRectTrans != null && toRectTrans != null)
        {
            Vector3[] fromWorldCorners = new Vector3[4];
            Vector3[] toLocalCorners = new Vector3[4];
            Matrix4x4 toLocal = to.worldToLocalMatrix;
            fromRectTrans.GetWorldCorners(fromWorldCorners);

            for (int i = 0; i < 4; i++)
                toLocalCorners[i] = toLocal.MultiplyPoint3x4(fromWorldCorners[i]);

            return new Rect(toLocalCorners[0].x, toLocalCorners[0].y, toLocalCorners[2].x - toLocalCorners[1].x,
            toLocalCorners[1].y - toLocalCorners[0].y);
        }

        return default(Rect);
    }
}