using System;
using UnityEngine;

[Serializable]
public class SpeedModel
{
    public float Current { get; private set; }

    public float WalkSpeed { get; }
    public float RunSpeed { get; }
    public float StrafeSpeed { get; }

    private float _targetSpeed;



    public SpeedModel(float walk, float run, float strafe)
    {
        WalkSpeed = walk;
        RunSpeed = run;
        StrafeSpeed = strafe;
       
        _targetSpeed = walk;
        Current = walk;
    }

    public void SetSprint(bool isSprinting)
    {
        _targetSpeed = isSprinting ? RunSpeed : WalkSpeed;
    }

    public void SetCustomTarget(float value)
    {
        _targetSpeed = value;
    }

    public void Tick(float deltaTime,float smooth = 1)
    {
        float newSpeed = Mathf.Lerp(Current, _targetSpeed, smooth * deltaTime);

        if (Mathf.Approximately(newSpeed, Current))
            return;

        Current = newSpeed;
    }
}
