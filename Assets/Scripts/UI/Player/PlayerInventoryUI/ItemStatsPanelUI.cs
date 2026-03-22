using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemStatsPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject wrapper;
    [SerializeField] private GameObject itemStatPanelsParent;

    [SerializeField] private GameObject itemStatPanelPrefab;

    private List<GameObject> itemsStatsPool = new List<GameObject>();


    public void GetPanel(IItemStats stats)
    {
        ClearPool();
        wrapper.SetActive(true);
        foreach(var stat in stats.ItemStats())
        {
            var panel = GetItemStatPanel();
            SetupItemStatPanel(panel,stat);
        }
    }

    public void HidePanel()
    {
        wrapper.SetActive(false);
    }

    private void ClearPool()
    {
        foreach(var item in itemsStatsPool)
        {
            item.SetActive(false);  
        }
    }

    private void SetupItemStatPanel(GameObject go, ItemStat stat)
    {
        var label = go.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        label.text = stat.key;
        var valueText = go.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        valueText.text = FormatStatValue(stat);
    }

    public GameObject GetItemStatPanel()
    {
        foreach (var item in itemsStatsPool)
        {
            if (!item.activeInHierarchy)
            {
                item.SetActive(true);
                return item;
            }
        }

        var go = Instantiate(itemStatPanelPrefab,itemStatPanelsParent.transform);
        itemsStatsPool.Add(go);
        return go;  
    }

    string FormatStatValue(ItemStat stat)
    {
        if(stat.formatType == ItemStatFormatType.percent)
        {
            return $"{stat.value * 100:0.#} %";   
        }

        return stat.value.ToString();
    }



}
