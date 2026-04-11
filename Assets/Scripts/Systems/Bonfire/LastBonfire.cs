using UnityEngine;

public class LastBonfire : Bonfire
{

    [SerializeField] private LevelInfoSO travelLevel;

    public override void Interact(IInteractor interactor)
    {
        if (travelLevel == null) return;

        interactor.StatsController.ResetAllStats();
        interactor.StatsModifier.ClearNegativeStatEffects();

        SaveLoadManager.Instance.TravelToLevel(travelLevel.levelName);

    }

}
