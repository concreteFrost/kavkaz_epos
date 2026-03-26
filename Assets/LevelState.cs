using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class LevelState
{
    public string levelId;

    public List<LootState> lootDatas = new List<LootState>(); 
    public List<CombatItemData> combatItemDatas = new List<CombatItemData>();
    //public List<BaseHumanoidAiServiceLocator> chracters = new List<BaseHumanoidAiServiceLocator>(); 


}
