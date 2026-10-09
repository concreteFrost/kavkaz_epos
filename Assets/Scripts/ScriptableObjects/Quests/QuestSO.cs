using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = ScriptablePaths.BASE_PATH + "/Quest System/Quest", fileName ="Quest_")]
public class QuestSO : WithIdSO
{
    public string questName;

    public List<ItemData> rewards = new List<ItemData>();

    public static Action<List<ItemData>> RewardsGranted;

    public virtual void GetRewards()
    {
        if (TryCreateRewards(out var items) && items.Count > 0)
            RewardsGranted?.Invoke(items);
    }



    public bool TryCreateRewards(out List<ItemData> items)
    {
        items = new List<ItemData>();
        foreach (var entry in rewards)
        {
            var data = entry?.CreateInstance();
            if (data == null || data.itemSO == null || data.quantity < 1)
            {
                Debug.LogWarning($"Quest '{name}' has a reward with no item or an invalid quantity.", this);
                items.Clear();
                return false;
            }
            items.Add(data);
        }
        return true;
    }
}