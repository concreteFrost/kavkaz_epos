using System.Collections;
using UnityEngine;

public class RockTrap : BaseTrap
{
    RollingRock[] rocks;
    [SerializeField] ParticleSystem dustParticle;

    [SerializeField] private float impulseForce = 50f;

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
        foreach (var rock in rocks)
        {
            rock.ActivateRock(impulseForce, transform.forward);
        }
    }

}
