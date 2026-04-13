using System.Collections.Generic;
using System;
using UnityEngine;

// Сохраняет текущее состояние арены и босса
[Serializable]
public class BossArenaState
{
    public string arenaId;
    public bool wasActivated;   // активирована ли арена
    public bool bossKilled;

}

public class BossArenaController : MonoBehaviour
{
    PlayerManager player;
    BossArenaActivator activator;
    
    BossArenaUI arenaUI;
    QuestCompletionTrigger questCompletionTrigger;

    [SerializeField] Transform bossSpawnPosition; // позиция спавна босса
    [SerializeField] GameObject bossPrefab;       // префаб босса
    [SerializeField] string bossName;

   

    [HideInInspector] public EnemyServiceLocator bossServices; // ссылки на сервисы босса
    EnemyBrain bossBrain;

    public BossArenaState state;
   
    public List<BossPhaseState> phases = new List<BossPhaseState>();

    //динамичные переменные
    BossPhaseState currentPhase;
    List<EnemySpecialAction> runtimeActions = new List<EnemySpecialAction>();
    EnemySpecialAction currentSpecialAction; // текущий спецприем

    public void Init()
    {
        activator = GetComponentInChildren<BossArenaActivator>();
        arenaUI = GetComponentInChildren<BossArenaUI>();
        questCompletionTrigger = GetComponent<QuestCompletionTrigger>();
       
        state.arenaId = GetComponent<UniqueId>().uniqueId;

        if (activator != null)
        {
            activator.ArenaEntered += OnArenaEntered;
        }


    }

    private void OnDisable()
    {
        if (activator != null)
        {
            activator.ArenaEntered -= OnArenaEntered;
        }
    }

    private void Update()
    {
        // если арена не активна или босс убит — ничего не делать
        if (!state.wasActivated || state.bossKilled) return;

        TryExecuteSpecial();
    }

    public void LoadData(BossArenaState loadState)
    {
        state.bossKilled = loadState.bossKilled;
        state.wasActivated = loadState.wasActivated;    

    }

    // Вызывается при входе игрока в арену
    private void OnArenaEntered()
    {
        if (state.wasActivated || state.bossKilled) return;

        // Спавн босса
        GameObject go = Instantiate(bossPrefab, bossSpawnPosition.position, Quaternion.identity);

        bossServices = go.GetComponent<EnemyServiceLocator>();
        bossServices.Init();

        bossBrain = bossServices.brain;

        player = FindAnyObjectByType<PlayerManager>();
        bossServices.fovController.SetLockedTarget(player.GetComponentInChildren<IDamagable>());

        // Подписка на события здоровья босса
        bossServices.statsManager.Health.CurrentChanged += OnBossHealthChanged;
        bossServices.statsManager.Health.Depleted += OnBossDeath;

        SetPhase(phases[0]);

        // Инициализация всех спецприемов босса
        foreach (var action in currentPhase.specialActions)
            action.Init();

        ShuffleSpecialActions();  // выбираем первый спецприем

        state.wasActivated = true;

        arenaUI.ShowPanel(bossName, bossServices.statsManager.Health.Current);
    }

    // Пытается выполнить текущий спецприем
    private void TryExecuteSpecial()
    {
        if (currentSpecialAction == null) return;
        if (currentSpecialAction.isProcessing) return; // уже выполняется
        if (!currentSpecialAction.IsReady()) return;   // ещё не готов по кулдауну

        StartCoroutine(currentSpecialAction.StartExecuteCoroutine(bossBrain, GetNextAction));
    }

    private void GetNextAction()
    {
        if(!currentSpecialAction.canRepeat) runtimeActions.Remove(currentSpecialAction);
        
        ShuffleSpecialActions();
    }

    // Выбирает случайный спецприем для следующего использования
    private void ShuffleSpecialActions()
    {
        if(runtimeActions.Count == 0)
        {
            currentSpecialAction = null;
            return;
        }
        
        var rnd = UnityEngine.Random.Range(0, runtimeActions.Count);
        currentSpecialAction = runtimeActions[rnd];
        currentSpecialAction.Init();
    }

    private float GetNormalizedHealth()
    {
        var health = bossServices.statsManager.Health;
        return health.Current / health.CurrentMax;
    }

    private void TryChangePhase()
    {
        float hp = GetNormalizedHealth();

        foreach (var phase in phases)
        {
            if (phase.IsInPhase(hp) && phase != currentPhase)
            {
                SetPhase(phase);
                break;
            }
        }
    }

    private void SetPhase(BossPhaseState newPhase)
    {
        currentSpecialAction?.specialMove.Exit();

        currentPhase = newPhase;

        runtimeActions = new List<EnemySpecialAction>(currentPhase.specialActions);

        foreach (var action in currentPhase.specialActions)
            action.Init();

        ShuffleSpecialActions();
    }

    // Обработка смерти босса
    private void OnBossDeath()
    {
        bossServices.statsManager.Health.CurrentChanged -= OnBossHealthChanged;
        bossServices.statsManager.Health.Depleted -= OnBossDeath;
        state.bossKilled = true;
        arenaUI.HidePanelWithDelay();

        foreach(var phase in phases)
        {
            foreach (var action in phase.specialActions)
            {
                action.specialMove.OnFightEnded();
            }
        }

        if (questCompletionTrigger != null)
        {
            questCompletionTrigger.Trigger();
        }
        else
        {
            Debug.Log("no quest completion trigger was found. quest completion will be ignored ");
        }

    }


    // Обновление UI здоровья
    private void OnBossHealthChanged(float current)
    {
        arenaUI.UpdateHealthSlider(current);
        TryChangePhase();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(bossSpawnPosition.position, .5f);
    }
}
