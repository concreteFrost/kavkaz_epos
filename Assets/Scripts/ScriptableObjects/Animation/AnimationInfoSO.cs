using UnityEngine;

[CreateAssetMenu(fileName = "AnimationInfo", menuName = "Scriptable Objects/Animation/AnimationInfo")]
public class AnimationInfoSO : ScriptableObject
{
    public AnimationClip clip;

    public float hitStartFrame;
    public float hitEndFrame;

    public float animationSpeed = 1f;
}
