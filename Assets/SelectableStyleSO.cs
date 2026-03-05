using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName ="Selectable Style", menuName = ScriptablePaths.UI_STYLES_PATH + "/Selectable Style")]
public class SelectableStyleSO : ScriptableObject
{
    public Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;
    public Color selectedColor;
    public Color disabledColor;

    public TMP_FontAsset font;
    public Color textColor;
}