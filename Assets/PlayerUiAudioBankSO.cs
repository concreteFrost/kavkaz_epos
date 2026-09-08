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


    [Header("Items")]
    public EventReference ev_item_grabbed;
    public EventReference ev_item_card_slide;


    
}
