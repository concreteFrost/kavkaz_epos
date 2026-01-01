using UnityEngine;

public class InputBuffer
{
    float bufferTime;
    float bufferDuration;

    public InputBuffer(float duration)
    {
        bufferDuration = duration;
    }

    public void Press()
    {
        bufferTime = Time.time;
    }

    public bool IsValid =>
        Time.time - bufferTime <= bufferDuration;

    public void Consume()
    {
        bufferTime = -999f;
    }

    public void Reset()
    {
        bufferTime = -999f;
    }
}
