using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CharacterInitializer : MonoBehaviour
{
    List<BaseHumanoidAiServiceLocator> allCharacters = new();

    public PlayerManager Player { get; private set; }
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerSpawnPosition;

    public bool canInitPlayer = true;

    //public List<EnemyBrain> brains = new();    

    private void Awake()
    {
        
        allCharacters = FindObjectsByType<BaseHumanoidAiServiceLocator>(FindObjectsSortMode.None).ToList();

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (canInitPlayer)
        {
            Player = Instantiate(playerPrefab, playerSpawnPosition).GetComponent<PlayerManager>();
            Player.Init();

        }


        foreach (var c in allCharacters)
        {
            c.Init();   
        }
    }

   

    private void OnDrawGizmos()
    {
        if (playerSpawnPosition == null) return;

        Gizmos.color = new Color(0f, 0f, 1f, 1f);
        Gizmos.DrawSphere(playerSpawnPosition.position, .5f);

    }
}
