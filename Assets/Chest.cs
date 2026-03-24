using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    Animator animator;
    public bool isOpened;
    [SerializeField] StaticLootHolder lootHolder;

    #region IInteractable Contract

    public ItemInteractionType InteractType() => ItemInteractionType.Chest;

    public bool CanInteract() => !isOpened;
    public bool HasInteracted { get; set; }
    public Vector3 InitialPosition {  get; set; }

    #endregion

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        InitialPosition = transform.position;
        animator = GetComponent<Animator>();
   
        lootHolder.Init();
        lootHolder.gameObject.SetActive(false);
        CloseChest();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!isOpened) OpenChest();
            else CloseChest();
        }
    }

    public  void Interact(ICollector collector)
    {
        if (!isOpened)
        {
            HasInteracted = true;
            lootHolder.gameObject.SetActive(true);
            OpenChest();
        }
        
    }

    public void OpenChest()
    {
        animator.SetBool("Open", true);
        isOpened = true;
    }

    public void CloseChest()
    {
        animator.SetBool("Open", false);
        isOpened = false;
        
    }
}
