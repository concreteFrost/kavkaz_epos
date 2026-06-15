using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionPanelUI : MonoBehaviour
{
    [SerializeField] ItemStatsPanelUI itemStatPanel;

    [SerializeField] GameObject wrapper;

    [Header("Common Info")]
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemDescriptionText;


    [Header("Status Effects Info")]
    [SerializeField] GameObject effectsPanelWrapper;
    [SerializeField] GameObject activeEffectInstancesParent;
    [SerializeField] GameObject cancelEffectInstancesParent;
    [SerializeField] GameObject effectInstancePrefab;
    private List<StatusEffectPanelUI> activeStatusEffectsPool = new List<StatusEffectPanelUI>();
    private List<Image> cancelsStatusEffectsPool = new List<Image>();   

    public void ShowPanel(ItemSO item)
    {
        wrapper.SetActive(true);


        ShowCommonInfo(item);
        DefineActivePanel(item);
    }

    public void HidePanel()
    {
        wrapper.SetActive(false);
        ClearCommonItemInfo();    
    }

    private void ShowCommonInfo(ItemSO item)
    {
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;
        itemIcon.sprite = item.itemImage;
    }

    public void ClearCommonItemInfo()
    {
        itemNameText.text =string.Empty;    
        itemDescriptionText.text = string.Empty;
        itemIcon.sprite = null;
    }



    private void DefineActivePanel(ItemSO item)
    {
        HideEffectsPanel();
 
        switch (item)
        {
            case StatModifierItemSO:
                var statModifierItem = (StatModifierItemSO)item;
                ShowEffectsPanel(); 
                SetupEffectsPanel(statModifierItem.effectData);
                break;
            case SpellProjectileSO:
                var spellItem = (SpellProjectileSO)item;
                ShowEffectsPanel();
                SetupEffectsPanel(spellItem.damageData.statusEffectData);
                break;
          
        }

        if (item is IItemStats itemStats)
        {
            
            itemStatPanel.GetPanel(itemStats);
        }
        else
        {
            itemStatPanel.HidePanel();
        }
    }

    #region Effects

    private void HideEffectsPanel() => effectsPanelWrapper.SetActive(false);
    private void ShowEffectsPanel()=> effectsPanelWrapper.SetActive(true);
    private void SetupEffectsPanel(StatusEffectData effects)
    {
        DisableAllEffects();


        foreach (var effect in effects.effects)
        {
            var ui = GetEffectInstance();
            ui.SetupEffectInfo(effect);
        }


        foreach (var effect in effects.effectsToCancel)
        {
            var ui = GetCancelEffectImage();
            ui.sprite = effect.effectImage;
        }
    }


    private void DisableAllEffects()
    {
        foreach (var effectUI in activeStatusEffectsPool)
        {
            effectUI.ClearEffectInfo();
            effectUI.gameObject.SetActive(false);
        }

        foreach(var effectUI in cancelsStatusEffectsPool)
        {
            effectUI.gameObject.SetActive(false);
        }
    }

    private StatusEffectPanelUI GetEffectInstance()
    {
        foreach (var effectUI in activeStatusEffectsPool)
        {
            if (!effectUI.gameObject.activeSelf)
            {
                effectUI.gameObject.SetActive(true);
                return effectUI;
            }
        }

        // если свободных нет Ч создаЄм новый
        var instance = Instantiate(effectInstancePrefab, activeEffectInstancesParent.transform);
        var ui = instance.GetComponent<StatusEffectPanelUI>();

        activeStatusEffectsPool.Add(ui);
        return ui;
    }

    private Image GetCancelEffectImage()
    {
        foreach(var img in cancelsStatusEffectsPool)
        {
            if (!img.gameObject.activeSelf)
            {
                img.gameObject.SetActive(true);
                return img; 
            }
        }

        var instance = Instantiate(new GameObject(), cancelEffectInstancesParent.transform);
        instance.AddComponent<Image>();
        var newImg = instance.GetComponent<Image>();   
        cancelsStatusEffectsPool.Add(newImg);
        return newImg;
    }

    #endregion




}
