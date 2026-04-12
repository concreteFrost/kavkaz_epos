using System.Collections.Generic;
using UnityEngine;


public class GlobalQuestManager : MonoBehaviour
{
    public static GlobalQuestManager Instance;

    [SerializeField] List<QuestSO> defaultQuests = new List<QuestSO>();
    public List<QuestInstance> allQuests = new List<QuestInstance>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);    
        }
    }


    public void Init()
    {
        if (allQuests.Count > 0) return;

        foreach (var questSO in defaultQuests)
        {
            StartNewQuest(questSO);
        }
    }

    public QuestInstance StartNewQuest(QuestSO questSO)
    {
    
        QuestInstance newQuest = new QuestInstance();
        newQuest.Init(questSO);

        allQuests.Add(newQuest);

        return newQuest;    
    }

    public void CompleteQuest(QuestSO questSO)
    {
        var targetQuest = allQuests.Find(x => x.definition.id == questSO.id);

        if(targetQuest == null)
        {
           targetQuest = StartNewQuest(questSO);
            
        }

        targetQuest.Complete();
    }

    public void GetCurrentQuestsState()
    {
        foreach (var quest in allQuests)
        {
            if(quest.state.isCompleted)
            {
               
                quest.Complete();
            }
               
        }
    }

    public bool IsQuestCompleted(QuestSO quest)
    {
        if(allQuests.Count == 0) return false;

        var targetQuest = allQuests.Find((x) => x.state.questId == quest.id);

        if(targetQuest == null) return false;   

        return targetQuest.state.isCompleted;
    }

    public List<QuestState> SaveQuestsState()
    {
        List<QuestState> questsToSave = new List<QuestState>();

        foreach(var quest in allQuests)
        {
            QuestState questState = new QuestState
            {
                questId = quest.state.questId,
                isCompleted = quest.state.isCompleted
            };

            questsToSave.Add(questState);   
        }

        return questsToSave;    
    }

    public void LoadQuestsData(SaveGameData data)
    {
        var questsData = data.questsStates;

        if (questsData.Count == 0) return;

        allQuests.Clear();

        var resources = Resources.LoadAll<QuestSO>("Systems/Quests/");

        foreach(var load in questsData)
        {
            foreach(var resourceQuest in resources)
            {
                if(load.questId == resourceQuest.id)
                {
                    QuestInstance loadedQuest = new QuestInstance();
                    loadedQuest.LoadQuest(resourceQuest, load.isCompleted);

                    allQuests.Add(loadedQuest); 
                }
            }
        }
    }



}
