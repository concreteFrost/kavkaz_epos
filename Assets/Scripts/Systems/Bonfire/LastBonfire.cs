using UnityEngine;

public class LastBonfire : Bonfire
{

    [SerializeField] private BiomInfoSO travelLevel;

    public override void Interact(IInteractor interactor)
    {
        if (travelLevel == null) return;

        interactor.StatsController.ResetAllStats();
        interactor.StatsModifier.ClearNegativeStatEffects();

        SceneTransitionManager.Instance.TravelToLevel(travelLevel.biomName, Vector3.zero);

    }

    public string GetDestinationName() => travelLevel?.biomName;

}
