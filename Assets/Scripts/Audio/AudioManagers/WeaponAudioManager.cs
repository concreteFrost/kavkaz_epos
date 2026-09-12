using FMODUnity;
using UnityEngine;

public class WeaponAudioManager 
{

    GameObject source;

    public WeaponAudioManager(GameObject source)
    {
        this.source = source;   
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlaySwing(EventReference ev)
    {
        if (ev.IsNull)
        {
            Debug.Log("WeaponAudioManager: Swing EventReference is not assigned.");
            return;
        }

        if (source == null)
        {
            Debug.Log("WeaponAudioManager: Source GameObject is null.");
            return;
        }

        AudioEventPlayer.Play3DOneShot(
            ev,
            source,
            "WeaponAction", 0);
    }


}
