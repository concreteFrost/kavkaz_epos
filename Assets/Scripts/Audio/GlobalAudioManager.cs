using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;
    private EventInstance currentMusic;

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

   

    public void PlayMusic(EventReference music)
    {
        StopMusic();

        currentMusic = AudioEventPlayer.Play2D(music);
    }

    public void StopMusic(bool fade = true)
    {
        if (!currentMusic.isValid())
            return;

        AudioEventPlayer.Stop(currentMusic, fade);
        currentMusic.clearHandle();
    }
}

