using UnityEngine;

public class EmitterTrap : BaseTrap
{
    GunEmitter emitter;

    [SerializeField] float maxResetTimer = 3;
    float currResetTimer;

    public override void Init()
    {
        base.Init();
        emitter = GetComponent<GunEmitter>();
        currResetTimer = 0;
    }

    //private void Start()
    //{
    //    Init();
    //}

    private void Update()
    {
        if (wasActivated) {

            currResetTimer += Time.deltaTime;

            if(currResetTimer >= maxResetTimer)
            {
                Deactivate();
            }
        }
    }
    public override void Deactivate()
    {
        wasActivated = false;
        currResetTimer = 0;
    }

    public override void Activate()
    {
        base.Activate();

        if(emitter == null)
        {
            emitter = GetComponent<GunEmitter>();
        }

        emitter.Emit();

    }

}
