using UnityEngine;
using UnityEngine.UI;

public class LockOnTargetUI : MonoBehaviour
{
	private Transform currentTarget;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();    
    }

    private void Start()
    {
        ResetTarget();    
    }

    public void CalculateImagePosition()
	{
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.position);

        // Проверяем, что цель видна камерой
        if (screenPos.z > 0)
        {
            // Конвертируем экранные координаты в локальные координаты RectTransform
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                img.canvas.transform as RectTransform,
                screenPos,
                img.canvas.worldCamera,
                out Vector2 localPos
            );

            img.rectTransform.localPosition = localPos;
        }
    }

    public void SetTarget(Transform _currTarget)
    {
        currentTarget = _currTarget;
        img.enabled = true;
    }

    public void ResetTarget()
    {
        currentTarget = null;   
        img.enabled = false;    
    }
}
