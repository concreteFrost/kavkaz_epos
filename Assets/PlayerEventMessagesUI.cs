using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerEventMessagesUI : MonoBehaviour
{
    [SerializeField] GameObject wrapper;
    [SerializeField] TextMeshProUGUI textMessage;

    Coroutine messageCoroutine;

    public void Init()
    {
        HidePanel();
    }

    private void OnEnable()
    {
        Door.DoorMessage += OnMessageReceived;
    }

    private void OnDisable()
    {
        Door.DoorMessage -= OnMessageReceived;
    }

    public void ShowPanel()
    {
        wrapper.SetActive(true);
    }

    public void HidePanel()
    {
        SetMessageText(string.Empty);
        wrapper.SetActive(true);
    }

    private void SetMessageText(string txt) => textMessage.text = txt;

    private void OnMessageReceived(string message)
    {
        if(messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            HidePanel();

            messageCoroutine = null;
        }

        messageCoroutine = StartCoroutine(ShowMessageCoroutine(message));
    }

    IEnumerator ShowMessageCoroutine(string message)
    {
        ShowPanel();
        SetMessageText(message);

        yield return new WaitForSeconds(5f);

        HidePanel();

        messageCoroutine = null;


    }
}
