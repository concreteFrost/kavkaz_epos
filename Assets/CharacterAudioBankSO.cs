using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName =ScriptablePaths.AUDIO_PATH + "/CharacterAudioBank", fileName ="character_audio_")]
public class CharacterAudioBankSO : ScriptableObject
{
    public EventReference ev_footsteps;
    public EventReference ev_landing;
}
