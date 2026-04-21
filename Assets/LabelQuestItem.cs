using TMPro;
using UnityEngine;

public class LabelQuestItem : MonoBehaviour
{
    [SerializeField] TextMeshPro text_questName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ToggleText(bool visible) => text_questName.gameObject.SetActive(visible);
    // Update is called once per frame
    public void SetText(string text) => text_questName.text = text;
}
