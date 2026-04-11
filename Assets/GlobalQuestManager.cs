using System;
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

    public void StartNewQuest(QuestSO questSO)
    {
        QuestInstance newQuest = new QuestInstance();
        newQuest.Init(questSO);

        allQuests.Add(newQuest);
    }

    public void CompleteQuest(QuestSO questSO)
    {
        var targetQuest = allQuests.Find(x => x.definition.id == questSO.id);

        if(targetQuest != null)
        {
            targetQuest.Complete();
        }
    }

    public void GetCurrentQuestsState()
    {
        foreach (var quest in allQuests)
        {
            if(quest.state.isCompleted)
            {
                Debug.Log("got completed quest");
                quest.Complete();
            }
               
        }
    }



}
