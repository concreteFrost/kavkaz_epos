using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UINavigationUtils 
{
    /// <summary>
    /// Настроить сеточную навигацию для списка кнопок с фиксированным количеством колонок.
    /// Если вниз или вправо нет элемента, ищет ближайший доступный элемент в следующем ряду/столбце.
    /// <param name="buttons">Список кнопок</param>
    /// <param name="columns">Кол-во элементов помещающихся в строку</param>
    /// </summary>
    public static void SetupGridNavigation(
        List<Selectable> buttons,
        int columns,
        List<Selectable> leftPanel = null)
    {
        if (buttons == null || buttons.Count == 0 || columns <= 0)
            return;

        for (int i = 0; i < buttons.Count; i++)
        {
            Navigation nav = buttons[i].navigation;
            nav.mode = Navigation.Mode.Explicit;

            int row = i / columns;
            int col = i % columns;

            // Вверх
            int upIndex = i - columns;
            nav.selectOnUp = (upIndex >= 0) ? buttons[upIndex] : null;

            // Вниз
            int downIndex = i + columns;
            nav.selectOnDown = (downIndex < buttons.Count) ? buttons[downIndex] : null;

            // Влево
            if (col > 0)
            {
                nav.selectOnLeft = buttons[i - 1];
            }
            else
            {
                // переход в левую панель
                if (leftPanel != null && leftPanel.Count > 0)
                {
                    int targetRow = Mathf.Clamp(row, 0, leftPanel.Count - 1);
                    nav.selectOnLeft = leftPanel[targetRow];
                }
                else
                {
                    nav.selectOnLeft = null;
                }
            }

            // Вправо
            int rightIndex = (col < columns - 1) ? i + 1 : -1;
            if (rightIndex >= buttons.Count) rightIndex = -1;
            nav.selectOnRight = (rightIndex >= 0) ? buttons[rightIndex] : null;

            buttons[i].navigation = nav;
        }
    }

    public static ColorBlock SetColorBlock(int currentCat, int expectedCat,Button b)
    {
        ColorBlock colorBlock = b.colors;
        colorBlock.normalColor = currentCat == expectedCat ? b.colors.highlightedColor : Color.white;
        return colorBlock;
    }



    /// <summary>
    /// Ограничивает навигацию между двумя вертикальными списками кнопок и кнопкой закрытия.
    /// </summary>
    public static void RecalculateVerticalLists(List<Button> leftList, List<Button> rightList, Button closeButton = null)
    {
        // собираем только активные кнопки
        var leftActive = leftList != null ? leftList.FindAll(b => b != null && b.gameObject.activeInHierarchy) : new List<Button>();
        var rightActive = rightList != null ? rightList.FindAll(b => b != null && b.gameObject.activeInHierarchy) : new List<Button>();

        // ЛЕВЫЙ список
        for (int i = 0; i < leftActive.Count; i++)
        {
            var btn = leftActive[i];
            var nav = new Navigation { mode = Navigation.Mode.Explicit };

            nav.selectOnUp = (i > 0) ? leftActive[i - 1] : null;
            nav.selectOnDown = (i < leftActive.Count - 1) ? leftActive[i + 1] : closeButton;

            if (rightActive.Count > 0) nav.selectOnRight = rightActive[0];

            btn.navigation = nav;
        }

        // ПРАВЫЙ список
        for (int i = 0; i < rightActive.Count; i++)
        {
            var btn = rightActive[i];
            var nav = new Navigation { mode = Navigation.Mode.Explicit };

            nav.selectOnUp = (i > 0) ? rightActive[i - 1] : null;
            nav.selectOnDown = (i < rightActive.Count - 1) ? rightActive[i + 1] : closeButton;

            if (leftActive.Count > 0) nav.selectOnLeft = leftActive[0];

            btn.navigation = nav;
        }

        // Кнопка закрыть: вверх -> последний активный (сначала правый, если нет – левый)
        if (closeButton != null)
        {
            var nav = new Navigation { mode = Navigation.Mode.Explicit };
            if (rightActive.Count > 0) nav.selectOnUp = rightActive[rightActive.Count - 1];
            else if (leftActive.Count > 0) nav.selectOnUp = leftActive[leftActive.Count - 1];
            closeButton.navigation = nav;
        }
    }

    public static void ClampVerticalNavigation(List<Selectable> btnList, List<Selectable> additionalPanel = null)
    {
        if (btnList == null || btnList.Count == 0) return;

        for (int i = 0; i < btnList.Count; i++)
        {
            var btn = btnList[i];
            var nav = btn.navigation;
            nav.mode = Navigation.Mode.Explicit;

            nav.selectOnUp = i > 0 ? btnList[i - 1] : null;
            nav.selectOnDown = i < btnList.Count - 1 ? btnList[i + 1] : null;
            nav.selectOnRight = additionalPanel != null ? additionalPanel[0] : null;
            nav.selectOnLeft = additionalPanel != null ? additionalPanel[0] : null;

            btn.navigation = nav;
        }
    }

    public static void ClampHorizontalNavigation(List<Selectable> btnList, List<Selectable> additionalPanel = null)
    {
        if (btnList == null || btnList.Count == 0) return;

        for (int i = 0; i < btnList.Count; i++)
        {
            var btn = btnList[i];
            var nav = btn.navigation;
            nav.mode = Navigation.Mode.Explicit;

            nav.selectOnLeft = i > 0 ? btnList[i - 1] : null;
            nav.selectOnRight = i < btnList.Count - 1 ? btnList[i + 1] : null;

            nav.selectOnUp = additionalPanel != null && additionalPanel.Count > 0
                ? additionalPanel[0]
                : null;

            nav.selectOnDown = additionalPanel != null && additionalPanel.Count > 0
                ? additionalPanel[0]
                : null;

            btn.navigation = nav;
        }
    }

    public static IEnumerator SelectWithDelay(GameObject go)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        var firstActive = go;

        if(firstActive != null)
        {
            EventSystem.current.SetSelectedGameObject(go);
        }

    }


    public static GameObject GetFirstActive(List<Selectable> selectables)
    {
        return selectables.FirstOrDefault(x => x.gameObject.activeInHierarchy).gameObject;
    }



}
