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
        if (rewards.Count == 0) return;

        RewardsGranted?.Invoke(rewards);    
    }


}