
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
        BonfireManager.TravelStarted += OnTravelStarted;
        Bonfire.BonfireDiscovered += OnBonfireDiscovered;
        CharacterLevelController.NewLevelReachedWithMessage += OnNewLevelReached;
        PlayerLevelControllerUI.LevelUpdated += OnPlayerLevelUpdated;
    }

    
   

    private void OnDisable()
    {
        PlayerInteractionController.LootCollected -= OnLootCollected;
        LootSmallPanelUI.ItemSlide -= OnItemSlide;
        PlayerUIManager.UiToggled -= OnUiToggled;
        BonfirePanelUI.BonfirePanelOpened -= OnBonfirePanelOpened;
        BonfireManager.TravelStarted -= OnTravelStarted;
        Bonfire.BonfireDiscovered -= OnBonfireDiscovered;
        CharacterLevelController.NewLevelReachedWithMessage -= OnNewLevelReached;
        PlayerLevelControllerUI.LevelUpdated -= OnPlayerLevelUpdated;
    }


    private void OnItemSlide() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_item_card_slide);

    private void OnLootCollected(ItemData data) => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_item_grabbed);

    private void OnUiToggled(bool isVisible)=> AudioEventPlayer.Play2DOneShot(isVisible ? audioBankSO.ev_menu_opened : audioBankSO.ev_menu_opened);

    private void OnBonfirePanelOpened() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_menu_opened);

    private void OnTravelStarted() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_travel_started);

    private void OnNewLevelReached() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_new_level_available);

    private void OnPlayerLevelUpdated() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_player_level_updated);

    private void OnBonfireDiscovered() => AudioEventPlayer.Play2DOneShot(audioBankSO.ev_bonfire_discovered);



}
