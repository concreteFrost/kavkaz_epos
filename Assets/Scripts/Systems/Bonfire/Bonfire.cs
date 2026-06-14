using System;
using UnityEngine;

[Serializable]
public class BonfireState
{
    public bool isDiscovered;
    public string bonfireId;
}
public class Bonfire : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject mesh;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Transform respawnPosition;
    [SerializeField] private string bonfireName;

    [HideInInspector]  public string id;

    public string GetBonfireName() => bonfireName;
    public Vector3 GetRespawnPosition() => respawnPosition.position;

    public bool isDiscovered;

    public static Action BonfireInteracted;

    #region IInteractable Contract
    public string InteractionName() => "Bonfire";

    public string ActionText() => "Activate";
    public bool HasInteracted { get => false; set => value = false; } // с этим предметом можно взаимодействовать всегда

    public ItemInteractionType InteractType() => ItemInteractionType.Item;

    public bool CanInteract() => !HasInteracted;
    #endregion

    public void Init()
    {
        id = GetComponent<UniqueId>().uniqueId;

        if (particles == null)
        {
            particles = GetComponentInChildren<ParticleSystem>();
        }

        particles.Stop();
    }

    public virtual void Interact(IInteractor interactor)
    {
        if (!isDiscovered)
        {
            DiscoverBonfire();
            return;
        }
       


        BonfireInteracted?.Invoke();
        GameStateManager.GameStateChanged?.Invoke(GameState.Bonfire);

        interactor.LifeCycleController.SetStartingPosition(respawnPosition.position);
        interactor.StatsController.ResetAllStats();
        interactor.StatsModifier.ClearNegativeStatEffects();

        SceneTransitionManager.Instance.SaveGame();

    }

    public void DiscoverBonfire()
    {
        particles.Play();
        isDiscovered = true; 
        BonfireManager.BonfireStatesUpdated?.Invoke();

    }

    public void ResetBonfireState()
    {
        isDiscovered = false;
        particles.Stop();
    }

    public void LoadData(BonfireState state)
    {
        isDiscovered = state.isDiscovered;

        if (isDiscovered)
        {
            particles.Play();
        }
    }



    private void OnDrawGizmos()
    {
        if (respawnPosition == null) return;

        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        Gizmos.DrawSphere(respawnPosition.position, .5f);

    }


}
