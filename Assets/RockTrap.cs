using UnityEngine;

public class RockTrap : BaseTrap
{
    RollingRock[] rocks;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Deactivate();
        }
    }

    public override void Init()
    {
       
        rocks = GetComponentsInChildren<RollingRock>();
        base.Init();
    }

    public override void Activate()
    {
        base.Activate();

        foreach(var rock in rocks)
        {
            rock.ActivateRock();    
        }
    }

    public override void Deactivate()
    {
        base.Deactivate();
        
        foreach(var rock in rocks)
        {
            rock.ResetRock();
        }
    }


}
