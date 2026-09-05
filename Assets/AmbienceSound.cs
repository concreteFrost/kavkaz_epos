using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AmbienceSound : MonoBehaviour
{
    [SerializeField] EventReference ev_ambience;
    [SerializeField] float soundRadius;
    
    [SerializeField] Color gizmoColor;
    [SerializeField] string gizmoText;
    [SerializeField] int fontSize = 18;

    private EventInstance ambienceInstance;

    

    void Start()
    {
        ambienceInstance = AudioEventPlayer.Play3D(ev_ambience, gameObject, "SoundDistance", soundRadius);
    }

    private void Update()
    {
        AudioEventPlayer.SetParameter(ambienceInstance, "SoundDistance", soundRadius);
    }

    void OnDestroy()
    {
        AudioEventPlayer.Stop(ambienceInstance, true);
    }

    private void OnDrawGizmos()
    {
        GizmoDrawer.DrawWithSphere(gizmoColor, transform, gizmoText, soundRadius, fontSize);
    }
}