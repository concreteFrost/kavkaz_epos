using UnityEngine;

public abstract class Item : MonoBehaviour
{


    public abstract void Init();

    public abstract void Interact(IInteractor interractor);



}
