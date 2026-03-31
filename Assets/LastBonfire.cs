using UnityEngine;
using UnityEngine.SceneManagement;

public class LastBonfire : Bonfire
{
    [SerializeField] private string targetScene;

    public override void Interact(IInteractor interactor)
    {
        interactor.StatsController.ResetAllStats();
        SaveLoadManager.Instance.TravelToLevel(targetScene);
    }

}
