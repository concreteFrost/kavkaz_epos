using FMOD.Studio;
using FMODUnity;
using System.Collections;
using UnityEngine;

public class RockTrap : BaseTrap
{
    RollingRock[] rocks;
    [SerializeField] ParticleSystem dustParticle;

    [SerializeField] private float impulseForce = 50f;

    [SerializeField] EventReference ev_qauke;

    private EventInstance quakeEventInstance;

    public override void Init()
    {
       

        rocks = GetComponentsInChildren<RollingRock>();

        foreach (var rock in rocks)
        {
            rock.Init();    
        }

        base.Init();
    }

    public override void Activate()
    {
        base.Activate();
      
        CameraShake.Shake?.Invoke(0.5f, 1, 3);

        quakeEventInstance = AudioEventPlayer.Play3D(ev_qauke,gameObject ,"QuakeState", 0);
        StartCoroutine(ActivateCoroutine());
    }

    public override void ResetState()
    {
        base.ResetState();
        
        foreach(var rock in rocks)
        {
            rock.ResetRock();
        }
    }

    public override void Deactivate()
    {
        foreach(var rock in rocks)
        {
            rock.HideRock();        
        }
    }

    IEnumerator ActivateCoroutine()
    {
        yield return new WaitForSeconds(1f);
        dustParticle.Play();
        quakeEventInstance.setParameterByName("QuakeState", 1);
        foreach (var rock in rocks)
        {
            rock.ActivateRock(impulseForce, transform.forward);
        }

        yield return new WaitForSeconds(5);
        AudioEventPlayer.StopAndRelease(quakeEventInstance, true);
    }

}
