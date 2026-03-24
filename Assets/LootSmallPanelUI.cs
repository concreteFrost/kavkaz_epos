using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootSmallPanelUI : MonoBehaviour
{
    [SerializeField] private Image lootImage;
    [SerializeField] private TextMeshProUGUI lootNameText;
    [SerializeField] private TextMeshProUGUI lootQuantityText;

    [SerializeField] private RectTransform slidePanel;

    [SerializeField] private float lifeTime = 4f;

    private Coroutine hideCoroutine;

    [SerializeField] private float slideDuration = 0.3f;
    [SerializeField] private float offsetX = -300f;


    private Vector2 targetPosition;

    private void Awake()
    {

        targetPosition = slidePanel.anchoredPosition;
    }

    public void SetLootData(ItemData lootData)
    {
        
        lootImage.sprite = lootData.itemSO.itemImage;
        lootNameText.text = lootData.itemSO.itemName;
        lootQuantityText.text = lootData.quantity.ToString();

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        if (!gameObject.activeSelf) return;

        hideCoroutine = StartCoroutine(HideAfterTime());
        StartCoroutine(SlideIn());
    }

    private IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);

        ClearLootData();
        gameObject.SetActive(false);
    }

    public void ClearLootData()
    {
        lootImage.sprite = null;
        lootNameText.text = null;
        lootQuantityText.text = null;
    }

    private IEnumerator SlideIn()
    {
        Vector2 startPos = targetPosition + new Vector2(offsetX, 0);
        slidePanel.anchoredPosition = startPos;

        float time = 0;

        while (time < slideDuration)
        {
            time += Time.deltaTime;
            float t = time / slideDuration;

            slidePanel.anchoredPosition = Vector2.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        slidePanel.anchoredPosition = targetPosition;
    }


}