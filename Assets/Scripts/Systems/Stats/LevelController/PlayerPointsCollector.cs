using System;
using UnityEngine;

public class PlayerPointsCollector : MonoBehaviour
{
    private CharacterLevelController levelController;

    public void Init(CharacterLevelController levelController)
    {
        this.levelController = levelController;

        PointsEmitter.PointsDropped += AddPoints;
    }

    private void OnDisable()
    {
        PointsEmitter.PointsDropped -= AddPoints;   
    }

  

    public void AddPoints(int points)
    {  
        levelController.AddXP(points);
    }
}
