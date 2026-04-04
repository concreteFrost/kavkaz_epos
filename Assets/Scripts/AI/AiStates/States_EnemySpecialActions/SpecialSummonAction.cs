using UnityEngine;

public class SpecialSummonAction : BaseAISpecialMove
{
    public GameObject summonPrefab;
    public Transform summonPosition;
    [SerializeField] private int summonAmount = 1;
    [SerializeField] private float summonSpread = 1f;

    public bool wasSummoned = false;
    public int summonAttempts = 0;
    public int maxSummonAttempts = 1;

    PlayerManager player;
    IDamagable playerDamagable;

    public override void Enter()
    {
        base.Enter();
        wasSummoned = false;

        if(player == null)
        {
            player = FindFirstObjectByType<PlayerManager>();
            playerDamagable = player.GetComponentInChildren<IDamagable>();  
        }
    }

    public override AIStateResult Run()
    {
        if(summonPosition == null)
        {
            Debug.Log("summoning position was not assigned. State was interrupted");
            return AIStateResult.Chase;
        }
        if (!wasSummoned && summonAttempts < maxSummonAttempts)
        {
            Summon();
            wasSummoned = true;
            summonAttempts++;   

            return AIStateResult.Chase; 
        }

        return AIStateResult.Chase;
    }

    public override void Exit()
    { 
        base.Exit();
        wasSummoned = true;
    }


    private void Summon()
    {
        for (int i = 0; i < summonAmount; i++)
        {
            GameObject go = Instantiate(summonPrefab, GetRandomSummonPosition(), Quaternion.identity);
            EnemyServiceLocator locator = go.GetComponent<EnemyServiceLocator>();
            locator.Init();

            if (playerDamagable != null)
                locator.fovController.SetLockedTarget(playerDamagable);

        }

       
    }

    private Vector3 GetRandomSummonPosition()
    {
        Vector3 randomOffset = Random.insideUnitSphere * summonSpread;
        randomOffset.y = 0; // оставляем на уровне земли
        return summonPosition.position + randomOffset;
    }
}