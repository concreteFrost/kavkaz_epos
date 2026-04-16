using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CharactersManager : MonoBehaviour
{
    public List<EnemyServiceLocator> enemies = new List<EnemyServiceLocator>();
    public List<FriendlyNpcServiceLocator> friendlyNpcs = new List<FriendlyNpcServiceLocator>();    
    public static Action CharacterStatesUpdated;

    public void Init()
    {
        enemies = GetComponentsInChildren<EnemyServiceLocator>().ToList();
        foreach (EnemyServiceLocator locator in enemies)
        {
            locator.Init();
        }

        friendlyNpcs = GetComponentsInChildren<FriendlyNpcServiceLocator>().ToList();  
        foreach(FriendlyNpcServiceLocator locator in friendlyNpcs)
        {
            locator.Init();
        }

        CharacterStatesUpdated?.Invoke();   
    }

    public CharactersState SaveCharacters()
    {
        CharactersState state = new CharactersState();
        state.enemyStates = SaveEnemies();
        state.friendlyNpcStates = SaveFriedlyNpcs();

        return state;
    }

    private List<EnemyState> SaveEnemies()
    {
        List<EnemyState> states = new List<EnemyState>();   
        foreach(var enemy in enemies)
        {
            EnemyState state = new EnemyState();
            
            Vector3 position = enemy.transform.position;

            state.enemyId = enemy.uid;

            state.enemyPosition[0] = position.x;
            state.enemyPosition[1] = position.y;
            state.enemyPosition[2] = position.z;

            state.statsData = enemy.statsManager.SaveStatsData();
            state.effectData = enemy.statsModifier.SaveEffectData();

            states.Add(state);  
        }

        return states;
    }

    private List<FriendlyNpcState> SaveFriedlyNpcs()
    {
        List<FriendlyNpcState> states = new List<FriendlyNpcState>();

        foreach (var npc in friendlyNpcs)
        {
            FriendlyNpcState state = new FriendlyNpcState();

            state.npcId = npc.uid;

            Vector3 position = npc.transform.position;
          
            state.npcPosition[0] = position.x;
            state.npcPosition [1] = position.y;
            state.npcPosition[2] = position.z;
          

            foreach (var dialogues in npc.dialogueController.dialogueStates)
            {
                var questId = dialogues.questDialogue.questToGiveSO.id;

                var dialogueState = new DialogueState()
                {
                    questId = questId,
                    wasQuestCompleted = dialogues.wasQuestCompleted,
                    wasQuestStarted = dialogues.wasQuestStarted,
                    wasRewardGiven = dialogues.wasRewardGiven,
                };

                state.npcQuestsState.Add(dialogueState);
            }

            states.Add(state);  

        }

        return states;
    }

   

    public void LoadCharactersData(LevelState levelState)
    {
        var enemieDatas = levelState.characterStates.enemyStates;

        foreach (var enemy in enemieDatas)
        {
            var match = enemies.Find((x) => x.uid == enemy.enemyId);

            if(match != null)
            {
                match.lifecycle.Respawn();

                match.transform.position = new Vector3(enemy.enemyPosition[0], enemy.enemyPosition[1], enemy.enemyPosition[2]);
                match.statsManager.LoadStatsData(enemy.statsData);
                match.statsManager.LoadStatsData(enemy.statsData);

           

                if(match.statsManager.Health.Current <= 0)
                {
                    match.lifecycle.PerformDeath();  
                }
               
            }
        }

        var friendlyNpcDatas = levelState.characterStates.friendlyNpcStates;

        foreach(var friendNpc in friendlyNpcDatas)
        {
            var match = friendlyNpcs.Find((x)=>x.uid == friendNpc.npcId);   

            if(match != null)
            {
                match.lifecycle?.Respawn();
                match.transform.position = new Vector3(friendNpc.npcPosition[0], friendNpc.npcPosition[1], friendNpc.npcPosition[2]);
                match.dialogueController.LoadData(friendNpc.npcQuestsState);

            }
        }

        CharacterStatesUpdated?.Invoke();
    }


    public void RespawnAllCharacters()
    {
        foreach(var character in enemies)
        {
            character.lifecycle.Respawn();
        }
    }

}
