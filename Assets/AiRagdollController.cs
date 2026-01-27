using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

public class CharacterBoneTransform
{

    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }

}

public class AiRagdollController : IRagdollController
{
    Animator anim;
    Rigidbody[] rigidbodies;
    NavMeshAgent agent;
    Transform self;

    Transform _hipsBone;
    Transform[] _bones;

    CharacterBoneTransform[] _faceupBoneTransforms;
    CharacterBoneTransform[] _facedownBoneTransforms;
    CharacterBoneTransform[] _ragdollBoneTransforms;

    float blendDuration = 0.5f;

    private bool isFacingUp;

    public bool IsRecovering {  get;  set; } 

    public AiRagdollController(Animator anim, NavMeshAgent agent, Rigidbody[] rbs, Transform self)
    {
        this.self = self;
        this.anim = anim;
        this.agent = agent;
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

    public void EnableRagdoll()
    {
        anim.enabled = false;
        agent.ResetPath();
        agent.enabled = false;

        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void DisableRagdoll()
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        anim.enabled = true;
        agent.enabled = true;
        agent.ResetPath();



    }

    public void Knockout()
    {
        IsRecovering = true;
        EnableRagdoll();
    }


    private void AlignRotationToHips()
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


    private void AlignPositionToHips()
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

    private CharacterBoneTransform[] GetStandUpBoneTransforms()
    {
        return isFacingUp ? _faceupBoneTransforms: _facedownBoneTransforms;
    }



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

        //Align();


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


    private void PopulateBoneTransforms(CharacterBoneTransform[] arr)
    {
        for (int i = 0; i < _bones.Length; i++)
        {
            arr[i].position = _bones[i].localPosition;
            arr[i].rotation = _bones[i].localRotation;
        }
    }

    private void SampleAnimationStartPose(string clipName, CharacterBoneTransform[] arr)
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
}

