using System;
using UnityEngine;

[Serializable]
public class SpeedModel : BaseStatModel
{
    public float WalkSpeed { get; }
    public float RunSpeed { get; }
    public float StrafeSpeed { get; }

    private float _targetSpeed;

    protected override float PerLevelBonus => 10f;
    protected override float DiminishFactor => 0.9f;


    public SpeedModel(float walk, float run, float strafe)
    {
        statType = global::StatType.Speed;

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

    public void Tick(float deltaTime,float smooth = 15)
    {
        float newSpeed = Mathf.Lerp(Current, _targetSpeed, smooth * deltaTime);

        if (Mathf.Approximately(newSpeed, Current))
            return;

        Current = newSpeed;
    }

    public override void ReduceCurrent(float value)
    {
        //без имплементации
    }

    public override void IncreaseCurrent(float value)
    {
        //без имплементации
    }
}
