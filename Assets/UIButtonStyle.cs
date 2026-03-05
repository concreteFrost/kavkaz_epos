using UnityEngine;
using TMPro;

public class UIButtonStyle : UISelectableStyle
{
    
    [SerializeField] private TMP_Text label;
    [SerializeField] private string labelText;
    [SerializeField] private float fontSize = 15f;
       
    protected override void ApplyStyle()
    {
        base.ApplyStyle();  

        if (label != null)
        {
            label.font = style.font;
            label.color = style.textColor;

            label.text = labelText;

            label.fontSize = fontSize;
        }
    }

}
