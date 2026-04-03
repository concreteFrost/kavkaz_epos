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
    public bool bossKilled;
    public bool wasActivated;

}
public class BossArenaManager : MonoBehaviour
{
    PlayerManager player;

    public BossArenaState state;
    [SerializeField] Transform bossSpawnPosition;
    [SerializeField] GameObject bossPrefab;
    BossArenaActivator activator;
    EnemyServiceLocator bossServices;
    EnemyBrain bossBrain;

    public List<BossSpecialAction> specialActions = new List<BossSpecialAction>();  


    private void Awake()
    {
        state = new BossArenaState();   
        activator  = GetComponentInChildren<BossArenaActivator>();  

       

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
        if (!state.wasActivated || state.bossKilled) return;

        TryExecuteSpecial();
    }

    private void OnArenaEntered()
    {
        if (state.wasActivated || state.bossKilled) return;

        GameObject go = Instantiate(bossPrefab, bossSpawnPosition.position, Quaternion.identity);

        bossServices = go.GetComponent<EnemyServiceLocator>();
        bossServices.Init();

        bossBrain = bossServices.brain;

        player = FindAnyObjectByType<PlayerManager>();
        bossServices.fovController.SetLockedTarget(player.GetComponentInChildren<IDamagable>());

        bossServices.statsManager.Health.CurrentChanged += OnBossHealthChanged;
        bossServices.statsManager.Health.Depleted += OnBossDeath;

        state.wasActivated = true;  
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
        
        Debug.Log("current health " +  current);    
    }

    private void OnBossDeath()
    {
        Debug.Log("boss died");
        bossServices.statsManager.Health.CurrentChanged -= OnBossHealthChanged;
        bossServices.statsManager.Health.Depleted -= OnBossDeath;
    }
}
