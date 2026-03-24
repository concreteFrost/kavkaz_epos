using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerLootPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject lootPanelPrefab;
    [SerializeField] private Transform lootPanelsParent;


    [SerializeField] private float showDelay = 0.3f;
    private List<LootSmallPanelUI> lootPanelsPool = new();
    private Queue<ItemData> lootQueue = new();
    private bool isProcessing;

    public void Init()
    {
        lootPanelsPool.Clear();
        lootQueue.Clear();
        PlayerItemsCollector.LootCollected += OnLootCollected;

    }

    private void OnDisable()
    {
        PlayerItemsCollector.LootCollected -= OnLootCollected;
    }

    private void OnLootCollected(ItemData data)
    {
        lootQueue.Enqueue(data);

        if (!isProcessing)
        {
            StartCoroutine(ProcessQueue()); 
        }

    }

    private LootSmallPanelUI GetSmallPanel()
    {
        foreach(var item in lootPanelsPool)
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
                return item;    
            }
        }

        GameObject go = Instantiate(lootPanelPrefab,lootPanelsParent);
        var panel = go.GetComponent<LootSmallPanelUI>();

        lootPanelsPool.Add(panel);
        return panel;

    }

    private IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while(lootQueue.Count > 0)
        {
            var data = lootQueue.Dequeue();

            var ui = GetSmallPanel();   
            ui.SetLootData(data);

            yield return new WaitForSeconds(showDelay);
        }

        isProcessing = false;   
    }
}
