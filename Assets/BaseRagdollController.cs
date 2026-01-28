using System;
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

    private MonoBehaviour context;

    protected Collider col;

    protected Animator anim;
    protected Rigidbody[] rigidbodies;
    protected Transform self;

    protected Transform _hipsBone;
    protected Transform[] _bones;

    protected CharacterBoneTransform[] _faceupBoneTransforms;
    protected CharacterBoneTransform[] _facedownBoneTransforms;
    protected CharacterBoneTransform[] _ragdollBoneTransforms;

    Coroutine recoveryCoroutine = null;

    protected float blendDuration = 0.5f;
    protected bool isFacingUp;

    #region IRagdollControlerContract

    public abstract void DisableRagdoll();
    public abstract void EnableRagdoll(float force, Transform from);
   
    public event Action KnockedOut;
    public event Action Recovered;

    public bool IsRecovering { get; set; }

    #endregion

    protected void Init(MonoBehaviour context, Animator anim, Rigidbody[] rbs, Transform self)
    {
        this.context = context;
        this.self = self;
        this.col = self.GetComponent<Collider>();
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

        SampleAnimationStartPose(AnimatorParameters.getUpClip, _faceupBoneTransforms);
        SampleAnimationStartPose(AnimatorParameters.getUpFromBellyClip, _facedownBoneTransforms);
        DisableRagdoll();
    }

    public void Knockout(float force, Transform from)
    {
        if (IsRecovering)
            return;

        IsRecovering = true;
        KnockedOut?.Invoke();

        EnableRagdoll(force,from);
        recoveryCoroutine = context.StartCoroutine(Recover());
    }

    public void ForceStop()
    {
        IsRecovering = false;

        if(recoveryCoroutine !=null)
           context.StopCoroutine(recoveryCoroutine);
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

    protected void ApplyImpulseFromSource(float force, Transform from)
    {
        Vector3 origin = from != null ? from.position : self.position - self.forward;
        Vector3 direction = (_hipsBone.position - origin).normalized;

        // немного вверх, чтобы не просто по земле
        direction.y = Mathf.Max(direction.y, 0.2f);
        direction.Normalize();

        Rigidbody hipsRb = _hipsBone.GetComponent<Rigidbody>();

        hipsRb.AddForce(direction * force, ForceMode.Impulse);
    }


    public IEnumerator Recover()
    {
        // 1. ∆дЄм минимальное врем€, чтобы ragdoll успел распастьс€
        yield return new WaitForSeconds(0.5f);

        // 2. ∆дЄм полной остановки всех rigidbody
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

        // 4. Ѕлендим к анимации
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


        DisableRagdoll();

        string animName = isFacingUp ? AnimatorParameters.getUpState : AnimatorParameters.getUpFromBellyState;
        anim.Play(animName);

        // ждЄм конца анимации
        yield return WaitForAnimationEnd(animName);

        Recovered?.Invoke();
        IsRecovering = false;
    }

    private IEnumerator WaitForAnimationEnd(string stateName, int layer = AnimatorParameters.damageLayer)
    {
        // ждЄм пока анимаци€ реально войдЄт в state
        while (!anim.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
            yield return null;

        // ждЄм пока анимаци€ не проиграетс€ до конца
        while (anim.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
            yield return null;
    }




}
