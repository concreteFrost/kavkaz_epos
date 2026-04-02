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

    //public List<EnemyBrain> brains = new();    

    private void Awake()
    {
        
        allCharacters = FindObjectsByType<BaseHumanoidAiServiceLocator>(FindObjectsSortMode.None).ToList();

        //foreach(var character  in allCharacters)
        //{
        //    var brain = character.GetComponentInChildren<EnemyBrain>();
        //    if (brain != null)
        //    {
        //        brains.Add(brain);
        //    }
        //}
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = Instantiate(playerPrefab, playerSpawnPosition).GetComponent<PlayerManager>();
        Player.Init();  
        

        foreach(var c in allCharacters)
        {
            c.Init();   
        }
    }

    //private void Update()
    //{
    //    if (brains.Count == 0) return;

    //    for (int i = brains.Count - 1; i >= 0; i--)
    //    {
    //        var brain = brains[i];

    //        if (brain == null)
    //        {
    //            brains.RemoveAt(i);
    //            continue;
    //        }

    //        float distance = Vector3.Distance(
    //            Player.serviceLocator.transform.position,
    //            brain.transform.position
    //        );

    //        if (distance < 20f)
    //        {
    //            brain.SetActivated(true);
    //            brains.RemoveAt(i); // удаляем после активации
    //        }
    //    }

    //}

    private void OnDrawGizmos()
    {
        if (playerSpawnPosition == null) return;

        Gizmos.color = new Color(0f, 0f, 1f, 1f);
        Gizmos.DrawSphere(playerSpawnPosition.position, .5f);

    }
}
