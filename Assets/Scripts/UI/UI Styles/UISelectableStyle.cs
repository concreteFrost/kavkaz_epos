using UnityEngine;
using UnityEngine.UI;

public class UISelectableStyle : MonoBehaviour
{
    [SerializeField] protected SelectableStyleSO style;
    protected Selectable selectable;

    protected virtual void ApplyStyle()
    {
        if (style == null) return;

        var colors = selectable.colors;
        colors.normalColor = style.normalColor;
        colors.highlightedColor = style.highlightedColor;
        colors.pressedColor = style.pressedColor;
        colors.selectedColor = style.selectedColor;
        selectable.colors = colors;
    }

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
        ApplyStyle();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            selectable = GetComponent<Selectable>();
            ApplyStyle();

        }


    }
}
