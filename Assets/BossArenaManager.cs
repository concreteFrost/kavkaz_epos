using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class BossSpecialAction
{

    public AIState<EnemyBrainContext> specialMove; // состояние для выполнения
    [Range(0, 1)] public float chance = 0.2f;      // вероятность выполнения
    public float cooldown = 5f;                   // минимальный интервал между срабатываниями
    [NonSerialized] public float lastTimeExecuted = -999f; // когда последний раз выполнялось

    public bool CanExecute()
    {
        return Time.time - lastTimeExecuted >= cooldown && UnityEngine.Random.value < chance;
    }

    public void MarkExecuted()
    {
        lastTimeExecuted = Time.time;
    }
}

[Serializable]
public class BossArenaState {
   
    public bool wasActivated;
    public BossInfo bossInfo;

}

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

    public BossArenaState state;
    [SerializeField] Transform bossSpawnPosition;
    [SerializeField] GameObject bossPrefab;

    [HideInInspector] public EnemyServiceLocator bossServices;
    
    EnemyBrain bossBrain;

    public List<BossSpecialAction> specialActions = new List<BossSpecialAction>();  


    private void Awake()
    {
       
        activator  = GetComponentInChildren<BossArenaActivator>();
        arenaUI = GetComponentInChildren<BossArenaUI>();

        if(activator != null)
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
        if (!state.wasActivated || state.bossInfo.bossKilled) return;

        TryExecuteSpecial();
    }

    private void OnArenaEntered()
    {
        if (state.wasActivated || state.bossInfo.bossKilled) return;

        GameObject go = Instantiate(bossPrefab, bossSpawnPosition.position, Quaternion.identity);

        bossServices = go.GetComponent<EnemyServiceLocator>();
        bossServices.Init();

        bossBrain = bossServices.brain;

        player = FindAnyObjectByType<PlayerManager>();
        bossServices.fovController.SetLockedTarget(player.GetComponentInChildren<IDamagable>());

        bossServices.statsManager.Health.CurrentChanged += OnBossHealthChanged;
        bossServices.statsManager.Health.Depleted += OnBossDeath;

        state.wasActivated = true;

        string bossName = state.bossInfo.bossName;
        float health = bossServices.statsManager.Health.Current;

        arenaUI.ShowPanel(state.bossInfo.bossName,health);
    }

    private void TryExecuteSpecial()
    {
        foreach (var action in specialActions)
        {
            if (action.CanExecute())
            {
                action.MarkExecuted();
                Debug.Log("special move executed");
                //// вставляем одноразово в FSM
                //var sp = action.specialMove.GetComponent<BossSpecialMove>();
                //sp.Init(bossBrain);
                
                //bossBrain.ForceChangeState(sp);
                break; // только одно действие за кадр
            }
        }
    }

    private void OnBossHealthChanged(float current)
    {
        
        arenaUI.UpdateHealthSlider(current);    
           
    }

    private void OnBossDeath()
    {
       
        bossServices.statsManager.Health.CurrentChanged -= OnBossHealthChanged;
        bossServices.statsManager.Health.Depleted -= OnBossDeath;

        arenaUI.HidePanelWithDelay();

       
    }
}
