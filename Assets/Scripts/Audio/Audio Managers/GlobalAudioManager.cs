using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public enum BiomMusicState
{
    Default = 0,
    BossFight = 1,
}

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;
    private EventInstance currentMusic;

    public EventInstance CurrentMusic() => currentMusic;
    private EventReference currentMusicEvent;

    private const string BIOM_MUSIC_STATE = "BiomMusicState";

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void PlayMusic(EventReference music, BiomMusicState state = BiomMusicState.Default)
    {
        if (music.IsNull)
        {
            StopMusic();
            return;
        }

        // Уже выбранную музыку не перезапускаем.
        if (currentMusic.isValid() &&
            currentMusicEvent.Guid.Equals(music.Guid))
        {
            return;
        }

        // Освобождаем предыдущий экземпляр, чтобы музыка разных биомов не накапливалась.
        StopMusic();

        currentMusic = AudioEventPlayer.Play2D(
            music, BIOM_MUSIC_STATE, (int)state);

        currentMusicEvent = music;
    }

    public void ChangeMusicState(BiomMusicState state)
    {
        if (!currentMusic.isValid()) return;

        currentMusic.setParameterByName(BIOM_MUSIC_STATE, (int)state);
    }

    public void StopMusic(bool fade = true)
    {
        if (!currentMusic.isValid())
            return;

        AudioEventPlayer.StopAndRelease(currentMusic, fade);

        currentMusic.clearHandle();
        currentMusicEvent = default;
    }

   
}

