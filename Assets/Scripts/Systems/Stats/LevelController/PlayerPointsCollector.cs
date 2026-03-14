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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddPoints(30);
        }
    }

    public void AddPoints(int points)
    {
        levelController.AddXP(points);
    }
}
