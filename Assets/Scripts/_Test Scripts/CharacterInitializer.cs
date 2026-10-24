using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CharacterInitializer : MonoBehaviour
{
    List<BaseHumanoidAiServiceLocator> allCharacters = new();

    public PlayerManager Player { get; private set; }



    //public List<EnemyBrain> brains = new();    

    private void Awake()
    {
        
        allCharacters = FindObjectsByType<BaseHumanoidAiServiceLocator>(FindObjectsSortMode.None).ToList();
      

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

     

        foreach (var c in allCharacters)
        {
            c.Init();   
        }
    }

}
