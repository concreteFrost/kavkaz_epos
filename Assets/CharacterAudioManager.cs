using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    [SerializeField] CharacterAudioBankSO bankSO;

    public void PlayWalk()
    {

        if (bankSO.ev_footsteps.IsNull)
        {
            Debug.Log("CharacterAuidoManager: no walk event assigned");
            return;
        }
        SurfaceType surface = SurfaceDetector.GetSurfaceType(
           transform.position,
           0.5f
       );

        Debug.Log("playing walk");
        AudioEventPlayer.Play3DOneShot(
            bankSO.ev_footsteps,
            gameObject,
            "SurfaceType",
            (int)surface
        );
    }

    public void PlayLanding()
    {
        if (bankSO.ev_landing.IsNull)
        {
            Debug.Log("CharacterAuidoManager: no landing event assigned");
            return;
        }

        SurfaceType surface = SurfaceDetector.GetSurfaceType(
         transform.position,
         1f
     );

        Debug.Log("playing landing");
        AudioEventPlayer.Play3DOneShot(
            bankSO.ev_landing,
            gameObject,
            "SurfaceType",
            (int)surface
        );
    }
}
