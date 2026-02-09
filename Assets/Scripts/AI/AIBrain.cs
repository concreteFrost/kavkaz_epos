using UnityEngine;

public abstract class AIBrain : MonoBehaviour, IBrain
{
    public abstract void ForceStop();
    public abstract void SetInitialState();
}