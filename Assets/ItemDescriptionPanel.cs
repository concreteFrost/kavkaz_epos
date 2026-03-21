using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionPanel : MonoBehaviour
{
    [SerializeField] GameObject wrapper;

    [Header("Common Info")]
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemDescriptionText;

    [Header("Weapon Info")]
    [SerializeField] GameObject weaponPanel;
    [SerializeField] TextMeshProUGUI weaponDamageText;
    [SerializeField] TextMeshProUGUI weaponDurabilityText;

    [Header("Shield Info")]
    [SerializeField] GameObject shieldPanel;
    [SerializeField] TextMeshProUGUI shieldDefenceBonusText;
    [SerializeField] TextMeshProUGUI shieldDurabilityText;

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
        DeactivateAllAdditionalPanels();    

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

    private void DeactivateAllAdditionalPanels()
    {
        weaponPanel.SetActive(false);
        shieldPanel.SetActive(false);
    }

    private void DefineActivePanel(ItemSO item)
    {
        HideEffectsPanel();
          
        switch (item)
        {
            case WeaponSO:
                weaponPanel.SetActive(true);
                SetupWeapon(item as WeaponSO);
                break;
            case ShieldSO:
                shieldPanel.SetActive(true);    
                SetupShield(item as ShieldSO);
                break;
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
    }

    private void SetupWeapon(WeaponSO weapon)
    {
        weaponDamageText.text = weapon.GetBaseDamage().ToString();
        weaponDurabilityText.text = weapon.breakdownPenalty.ToString();
    }

    private void SetupShield(ShieldSO shield)
    {
        shieldDefenceBonusText.text = shield.defenceBonus.ToString();
        shieldDurabilityText.text = shield.breakdownPenalty.ToString();
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
