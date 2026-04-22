using System;
using TMPro;
using UnityEngine;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] TextMeshProUGUI text_IntectionName;
    [SerializeField] TextMeshProUGUI text_InteractionAction;

    PlayerInteractionController interactionController;

    public void Init(PlayerInteractionController interactionController) 
    {
        this.interactionController = interactionController; 
        TogglePanel(false);

        this.interactionController.InteractionAvailable += OnInteractionAvailable;
        this.interactionController.InteractionLost += OnInteractionLost;

    }

    private void OnDisable()
    {
        if(interactionController != null)
        {
            interactionController.InteractionAvailable -= OnInteractionAvailable;
            interactionController.InteractionLost -= OnInteractionLost;
        }

    }

    public void TogglePanel(bool isVisible)=>wrapper.SetActive(isVisible);  

    private void OnInteractionAvailable(IInteractable target)
    {
        TogglePanel(true);

        text_IntectionName.text = target.InteractionName();
        text_InteractionAction.text = target.ActionText();
        
    }

    private void OnInteractionLost()
    {
        text_IntectionName.text = string.Empty;
        text_InteractionAction.text= string.Empty;

        TogglePanel(false);
    }

   
}