using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "audio_bank_player_ui", menuName = ScriptablePaths.AUDIO_PATH + "/Player UI Audio Bank")]
public class PlayerUiAudioBankSO : ScriptableObject
{


    [Header("Menu")]
    public EventReference ev_menu_opened;
    public EventReference ev_menu_section_changed;

    //public EventReference ev_menu_icon_clicked;

    //public EventReference ev_menu_icon_hover;


    [Header("Game Events")]
    public EventReference ev_item_grabbed;
    public EventReference ev_item_card_slide;
    public EventReference ev_travel_started;
    public EventReference ev_new_level_available;
    public EventReference ev_player_level_updated;
    public EventReference ev_bonfire_discovered;
   


    
}
