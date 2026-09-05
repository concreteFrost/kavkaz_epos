using System;
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

        
        AudioEventPlayer.Play3DOneShot(
            bankSO.ev_landing,
            gameObject,
            "SurfaceType",
            (int)surface
        );
    }

    public void PlayDamage(int damageType)
    {
        if (bankSO.ev_stab.IsNull)
        {
            Debug.Log("CharacterAudioManager: no damage event assigned");
            return;
        }
        AudioEventPlayer.Play3DOneShot(
            bankSO.ev_stab,
            gameObject,
            "StabType",
            damageType
            );
    }

    #region Voice Sounds


    private void PlayVoice(int voiceType)
    {
        if (bankSO.ev_voices.IsNull)
        {
            Debug.Log("CharacterAudioManager: no voice sounds assigned");
            return;
        }

        AudioEventPlayer.Play3DOneShot(bankSO.ev_voices, gameObject, "CharacterVoiceReaction", voiceType);
    }

    public void PlayAlert() => PlayVoice(0);
    public void PlayGetHit() => PlayVoice(1);

    public void PlayAttack() => PlayVoice(2);

    public void PlayPowerAttack() => PlayVoice(3);

    public void PlayDeath() => PlayVoice(4);

    internal void PlayJump() => PlayVoice(5);

    #endregion
}
