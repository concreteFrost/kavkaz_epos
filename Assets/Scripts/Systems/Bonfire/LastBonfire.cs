using UnityEngine;

public class LastBonfire : Bonfire
{

    [SerializeField] private BiomInfoSO travelLevel;

    public override void Interact(IInteractor interactor)
    {
        if (travelLevel == null) return;

        interactor.StatsController.ResetAllStats();
        interactor.StatsModifier.ClearNegativeStatEffects();

        SaveLoadManager.Instance.TravelToLevel(travelLevel.biomName);

    }

    public string GetDestinationName() => travelLevel?.biomName;

}
