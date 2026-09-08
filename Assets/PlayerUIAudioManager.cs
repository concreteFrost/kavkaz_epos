
using System;
using UnityEngine;

public class PlayerUIAudioManager : MonoBehaviour
{

    [SerializeField] PlayerUiAudioBankSO audioBankSO;

    private void OnEnable()
    {
        PlayerInteractionController.LootCollected += OnLootCollected;
        LootSmallPanelUI.ItemSlide += OnItemSlide;
        PlayerUIManager.UiToggled += OnUiToggled;
        BonfirePanelUI.BonfirePanelOpened += OnBonfirePanelOpened;
    }

   

    private void OnDisable()
    {
        PlayerInteractionController.LootCollected -= OnLootCollected;
        LootSmallPanelUI.ItemSlide -= OnItemSlide;
        PlayerUIManager.UiToggled -= OnUiToggled;
        BonfirePanelUI.BonfirePanelOpened -= OnBonfirePanelOpened;
    }


    private void OnItemSlide() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_item_card_slide);

    private void OnLootCollected(ItemData data) => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_item_grabbed);

    private void OnUiToggled(bool isVisible)=> AudioEventPlayer.Play2DOneShot(isVisible ? audioBankSO.ev_menu_opened : audioBankSO.ev_menu_opened);

    private void OnBonfirePanelOpened() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_menu_opened);



}
