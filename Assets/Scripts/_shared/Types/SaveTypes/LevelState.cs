using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class LevelState
{
    public string levelId;
    
    public List<LootState> staticLootStates = new List<LootState>(); 
    public List<DynamicLootState> dynamicLootStated = new List<DynamicLootState>();

    public List<TrapState> trapStates  = new List<TrapState>();  
  
    public List<BonfireState> bonfireStates = new List<BonfireState>();
    public List<BossArenaState> bossArenaStates = new List<BossArenaState>();

    public CharactersState characterStates;
    public HubState hubState;   
    //public List<CombatItemData> combatItemDatas = new List<CombatItemData>();
    //public List<BaseHumanoidAiServiceLocator> chracters = new List<BaseHumanoidAiServiceLocator>(); 


}
