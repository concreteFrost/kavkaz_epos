using System;
using UnityEngine;

public class PointsEmitter : MonoBehaviour
{
    public int points;

    public static Action<int> PointsDropped;

    public void DropPoints()
    {
        PointsDropped?.Invoke(points);
    }
}
