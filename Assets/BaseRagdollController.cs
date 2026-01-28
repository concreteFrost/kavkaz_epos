using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterBoneTransform
{
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }

}

public abstract class BaseRagdollController : IRagdollController
{
    protected Animator anim;
    protected Rigidbody[] rigidbodies;
    protected Transform self;

    protected Transform _hipsBone;
    protected Transform[] _bones;

    protected CharacterBoneTransform[] _faceupBoneTransforms;
    protected CharacterBoneTransform[] _facedownBoneTransforms;
    protected CharacterBoneTransform[] _ragdollBoneTransforms;

    protected float blendDuration = 0.5f;
    protected bool isFacingUp;

    #region IRagdollControlerContract
    public bool IsRecovering { get; set; }

    public abstract void DisableRagdoll();
    public abstract void EnableRagdoll();
    public abstract void Knockout();
    public IEnumerator Recover()
    {
        // 1. Ждём минимальное время, чтобы ragdoll успел распасться
        yield return new WaitForSeconds(0.5f);

        // 2. Ждём полной остановки всех rigidbody
        bool moving = true;
        while (moving)
        {
            moving = false;
            foreach (var rb in rigidbodies)
            {
                if (rb.linearVelocity.sqrMagnitude > 0.01f)
                {
                    moving = true;
                    break;
                }
            }
            yield return null;
        }

        isFacingUp = _hipsBone.forward.y > 0;

        AlignRotationToHips();
        AlignPositionToHips();

        PopulateBoneTransforms(_ragdollBoneTransforms);

        // 4. Блендим к анимации
        float timer = 0f;

        CharacterBoneTransform[] standUpBoneTransforms = GetStandUpBoneTransforms();

        while (timer < blendDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / blendDuration);

            for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
            {
                _bones[boneIndex].localPosition = Vector3.Lerp(
                _ragdollBoneTransforms[boneIndex].position,
                standUpBoneTransforms[boneIndex].position, t
                );

                _bones[boneIndex].localRotation = Quaternion.Lerp(
                    _ragdollBoneTransforms[boneIndex].rotation,
                    standUpBoneTransforms[boneIndex].rotation,
                    t);
            }

            yield return null;
        }


        // 5. Завершение: отключаем ragdoll, включаем анимацию


        DisableRagdoll();

        if (isFacingUp)
            anim.Play("Get Up");

        else
            anim.Play("Get Up From Belly");

        IsRecovering = false;

    }

    #endregion

    protected void Init(Animator anim,  Rigidbody[] rbs, Transform self)
    {
        this.self = self;
        this.anim = anim;
        this.rigidbodies = rbs;

        _hipsBone = anim.GetBoneTransform(HumanBodyBones.Hips);
        _bones = _hipsBone.GetComponentsInChildren<Transform>();

        _faceupBoneTransforms = new CharacterBoneTransform[_bones.Length];
        _facedownBoneTransforms = new CharacterBoneTransform[_bones.Length];
        _ragdollBoneTransforms = new CharacterBoneTransform[_bones.Length];

        for (int i = 0; i < _bones.Length; i++)
        {
            _faceupBoneTransforms[i] = new CharacterBoneTransform();
            _facedownBoneTransforms[i] = new CharacterBoneTransform();
            _ragdollBoneTransforms[i] = new CharacterBoneTransform();
        }

        SampleAnimationStartPose("Getup", _faceupBoneTransforms);
        SampleAnimationStartPose("Getup_from_belly", _facedownBoneTransforms);
        DisableRagdoll();
    }

    protected void PopulateBoneTransforms(CharacterBoneTransform[] arr)
    {
        for (int i = 0; i < _bones.Length; i++)
        {
            arr[i].position = _bones[i].localPosition;
            arr[i].rotation = _bones[i].localRotation;
        }
    }


    protected void SampleAnimationStartPose(string clipName, CharacterBoneTransform[] arr)
    {
        Vector3 pos = self.position;
        Quaternion rot = self.rotation;

        var clip = anim.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == clipName);

        clip.SampleAnimation(self.gameObject, 0);
        PopulateBoneTransforms(arr);

        self.position = pos;
        self.rotation = rot;
    }

    protected void AlignRotationToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Quaternion originalHipsRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up;

        if (isFacingUp)
        {
            desiredDirection *= -1;
        }

        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(self.forward, desiredDirection);
        self.rotation *= fromToRotation;

        _hipsBone.position = originalHipsPosition;
        _hipsBone.rotation = originalHipsRotation;
    }


    protected void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        self.position = _hipsBone.position;

        Vector3 positionOffset = GetStandUpBoneTransforms()[0].position;
        positionOffset.y = 0;
        positionOffset = self.rotation * positionOffset;
        self.position -= positionOffset;

        if (Physics.Raycast(self.position, Vector3.down, out RaycastHit hitInfo))
        {
            self.position = new Vector3(self.position.x, hitInfo.point.y, self.position.z);
        }

        _hipsBone.position = originalHipsPosition;
    }

    protected CharacterBoneTransform[] GetStandUpBoneTransforms()
    {
        return isFacingUp ? _faceupBoneTransforms : _facedownBoneTransforms;
    }



}
