using UnityEngine;

[CreateAssetMenu(fileName = "AnimationInfo", menuName = ScriptablePaths.ANIMATION_PATH + "/AnimationInfo")]
public class AnimationInfoSO : ScriptableObject
{
    public AnimationClip clip;

    public float hitStartFrame;
    public float hitEndFrame;

    public float invincibleStartFrame;
    public float invincibleEndFrame;

    public float animationSpeed = 1f;
}
