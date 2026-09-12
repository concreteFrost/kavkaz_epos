using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class UiItemAudioEvent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, ISubmitHandler, ISelectHandler
{

    [Header("Audio Events")]
    [SerializeField] EventReference ev_hover;
    [SerializeField] EventReference ev_click;

    [SerializeField] EventReference ev_submit;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!eventData.eligibleForClick) return;

        if(eventData.clickCount >1)
        {
            AudioEventPlayer.Play2DOneShot(ev_submit);
            return;
        }

        AudioEventPlayer.Play2DOneShot(ev_click);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioEventPlayer.Play2DOneShot(ev_hover);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioEventPlayer.Play2DOneShot(ev_hover);
    }

    public void OnSubmit(BaseEventData eventData)
    {

        AudioEventPlayer.Play2DOneShot(ev_submit);
    }

}
