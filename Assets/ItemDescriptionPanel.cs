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
    [SerializeField] GameObject effectInstancesParent;
    [SerializeField] GameObject effectInstancePrefab;
    private List<StatusEffectPanelUI> statusEffectsPool = new List<StatusEffectPanelUI>();



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
                SetupEffectsPanel(statModifierItem.effectData.effects);
                break;
            case SpellProjectileSO:
                var spellItem = (SpellProjectileSO)item;
                ShowEffectsPanel();
                SetupEffectsPanel(spellItem.damageData.statusEffectData.effects);
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
    private void SetupEffectsPanel(List<StatusEffectEntry> effects)
    {
        DisableAllEffects();

        if (effects == null || effects.Count == 0)
        {
            effectInstancesParent.SetActive(false);
            return;
        }

        effectInstancesParent.SetActive(true);

        foreach (var effect in effects)
        {
            var ui = GetEffectInstance();
            ui.SetupEffectInfo(effect);
        }
    }


    private void DisableAllEffects()
    {
        foreach (var effectUI in statusEffectsPool)
        {
            effectUI.ClearEffectInfo();
            effectUI.gameObject.SetActive(false);
        }
    }

    private StatusEffectPanelUI GetEffectInstance()
    {
        foreach (var effectUI in statusEffectsPool)
        {
            if (!effectUI.gameObject.activeSelf)
            {
                effectUI.gameObject.SetActive(true);
                return effectUI;
            }
        }

        // если свободных нет Ч создаЄм новый
        var instance = Instantiate(effectInstancePrefab, effectInstancesParent.transform);
        var ui = instance.GetComponent<StatusEffectPanelUI>();

        statusEffectsPool.Add(ui);
        return ui;
    }

    #endregion




}
