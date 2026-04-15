using TMPro;
using UnityEngine;

public class DialogueManagerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_NpcName;
    [SerializeField] private TextMeshProUGUI text_dialogueContext;

    [SerializeField] private GameObject wrapper;

    private void Awake()
    {
        if (wrapper.gameObject.activeInHierarchy)
        {
            wrapper.SetActive(false);
        }
    }

    private void OnEnable()
    {
        DialogueController.DialogueStarted += OnDialogueStarted;
        DialogueController.DialogueProceed += OnDialogueProceed;
        DialogueController.DialogueCompleted += OnDialogueCompleted;
    }

    private void OnDisable()
    {
        DialogueController.DialogueStarted -= OnDialogueStarted;
        DialogueController.DialogueProceed -= OnDialogueProceed;
        DialogueController.DialogueCompleted -= OnDialogueCompleted;
    }

    private void OnDialogueStarted(string npcName)
    {
        wrapper.SetActive(true);

        text_NpcName.text = npcName;
        text_dialogueContext.text = string.Empty;
    }

    private void OnDialogueProceed(string line)
    {
        text_dialogueContext.text = line;
    }

    private void OnDialogueCompleted()
    {
        wrapper.SetActive(false);

        text_NpcName.text = string.Empty;
        text_dialogueContext.text = string.Empty;
    }
}