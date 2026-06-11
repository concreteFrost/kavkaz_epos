using UnityEngine;

public class GlobalUIManager : MonoBehaviour
{
    [SerializeField] private BonfirePanelUI bonfirePanelUI;
    [SerializeField] private PlayerLootPanelUI lootPanelUI;
    [SerializeField] private ScreenFaderUI screenFaderUI;   

    public static GlobalUIManager Instance;


    private void OnEnable()
    {
        GameStateManager.GameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameStateManager.GameStateChanged -= OnGameStateChanged;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        lootPanelUI.Init();
    }

    private void OpenBonfirePanel()
    {
        SetCursorState(true);
        bonfirePanelUI.ToggleMainPanel(true);
    }


    public void HideTravelPanel()
    {
        bonfirePanelUI.HideTravelPanel(true);
    }

    private void OnGameStateChanged(GameState state)
    {
        if(state == GameState.Game)
        {
            CloseAllPanels();
            return;
        }

        if (state == GameState.Bonfire)
        {
            OpenBonfirePanel();
            return;

        }

    }

    private void CloseAllPanels()
    {
        SetCursorState(false);
        bonfirePanelUI.HideAllPanels();
    }

    private void SetCursorState(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible
            ? CursorLockMode.None
            : CursorLockMode.Locked;
    }
}
