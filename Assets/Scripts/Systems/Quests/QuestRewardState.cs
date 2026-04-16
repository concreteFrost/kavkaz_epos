using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestRewardState
{
    public List<ItemData> rewards = new List<ItemData>();
    public bool wasRewardGiven;
}

