using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class LevelState
{
    public string levelId;

    public List<LootState> lootData = new List<LootState>(); 
    //public List<CombatItem> weaponState = new List<CombatItem>();
    //public List<BaseHumanoidAiServiceLocator> chracters = new List<BaseHumanoidAiServiceLocator>(); 


}
