using System.Collections.Generic;
using System;
using UnityEngine;

// Сохраняет текущее состояние арены и босса
[Serializable]
public class BossArenaState
{
    public bool wasActivated;   // активирована ли арена
    public BossInfo bossInfo;   // информация о боссе
}

// Информация о боссе
[Serializable]
public class BossInfo
{
    public string bossName;
    public HealthModel bossHealth;
    public bool bossKilled;
}

public class BossArenaManager : MonoBehaviour
{
    PlayerManager player;
    BossArenaActivator activator;
    public BossArenaUI arenaUI;
  
    [SerializeField] Transform bossSpawnPosition; // позиция спавна босса
    [SerializeField] GameObject bossPrefab;       // префаб босса

    [HideInInspector] public EnemyServiceLocator bossServices; // ссылки на сервисы босса
    EnemyBrain bossBrain;

    public BossArenaState state;
    public List<BossPhaseState> phases = new List<BossPhaseState>();
    

    //динамичные переменные
    BossPhaseState currentPhase;
    List<EnemySpecialAction> runtimeActions = new List<EnemySpecialAction>();
    EnemySpecialAction currentSpecialAction; // текущий спецприем

    private void Awake()
    {
        activator = GetComponentInChildren<BossArenaActivator>();
        arenaUI = GetComponentInChildren<BossArenaUI>();

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
        if (!state.wasActivated || state.bossInfo.bossKilled) return;

        TryExecuteSpecial();
    }

    // Вызывается при входе игрока в арену
    private void OnArenaEntered()
    {
        if (state.wasActivated || state.bossInfo.bossKilled) return;

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

        arenaUI.ShowPanel(state.bossInfo.bossName, bossServices.statsManager.Health.Current);
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
        state.bossInfo.bossKilled = true;
        arenaUI.HidePanelWithDelay();

        foreach(var phase in phases)
        {
            foreach (var action in phase.specialActions)
            {
                action.specialMove.OnFightEnded();
            }
        }
    }


    // Обновление UI здоровья
    private void OnBossHealthChanged(float current)
    {
        arenaUI.UpdateHealthSlider(current);
        TryChangePhase();
    }

}
