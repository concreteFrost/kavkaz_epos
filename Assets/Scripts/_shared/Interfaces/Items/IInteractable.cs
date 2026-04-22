using UnityEngine;


public enum ItemInteractionType
{
    Item = 0,
    Chest = 1,
    Door = 2,
    NPC = 3,
}

public interface IInteractable
{
    // имя предмета , нпс и тд
    string InteractionName();

    // отображаемый текст взаимодействия (подобрать, говорить и тд)
    string ActionText();

    // определяет анимацию взаимодействия
    ItemInteractionType InteractType();

    public bool HasInteracted { get; set; }
    bool CanInteract();

    void Interact(IInteractor picker);

}

