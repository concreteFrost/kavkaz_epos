using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBoneTransform
{
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }

}

public abstract class BaseRagdollController : IRagdollController
{

    private MonoBehaviour context; // ссылка дл€ использовани€ корутин

    protected Collider col; // основной коллайдер персонажа

    protected Animator anim;
    protected List<Rigidbody> rigidbodies = new List<Rigidbody>(); //rigidbodies относ€щиес€ к ragdoll
    protected Transform self;

    protected Transform _hipsBone; // центральна€ кость (таз)
    protected Transform[] _bones; // кости относ€щиес€ к ragdoll

    protected CharacterBoneTransform[] _faceupBoneTransforms;
    protected CharacterBoneTransform[] _facedownBoneTransforms;
    protected CharacterBoneTransform[] _ragdollBoneTransforms;

    Coroutine recoveryCoroutine = null;

    protected float blendDuration = 0.5f; // врем€ перехода между ragdoll и анимацией
    protected bool isFacingUp; //на какую сторону упал персонаж (живот/спина)
    #region IRagdollControlerContract

    public abstract void DisableRagdoll();
    public abstract void EnableRagdoll(float force, Transform from);

    //public Action KnockedOut;
    public event Action Recovered;
    public event Action RecoveredInInvalidArea;

    #endregion

    
    protected void InvokeRecover() => Recovered?.Invoke(); //обЄртка дл€ вызова в дочерних классах
    protected void InvokeInvalidRecover() => RecoveredInInvalidArea?.Invoke(); //обЄртка дл€ вызова в дочерних классах

    protected void Init(MonoBehaviour context, Animator anim, Transform self)
    {
        this.context = context;
        this.self = self;
        this.col = self.GetComponent<Collider>();
        this.anim = anim;
        _hipsBone = anim.GetBoneTransform(HumanBodyBones.Hips);

        InitBones();
        AddRigidbodies();
       
        SampleAnimationStartPose(AnimatorParameters.getUpClip, _faceupBoneTransforms);
        SampleAnimationStartPose(AnimatorParameters.getUpFromBellyClip, _facedownBoneTransforms);
        DisableRagdoll();
    }

    /// <summary>
    /// Initializes bone transform arrays for face-up, face-down, and ragdoll states using the child transforms of the
    /// hips bone.
    /// </summary>
    private void InitBones()
    {
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
    }

    /// <summary>
    /// ƒобавл€ет rigidbodies только на которых есть joints
    /// </summary>
    private void AddRigidbodies()
    {

        rigidbodies.Add(_hipsBone.GetComponent<Rigidbody>());

        rigidbodies.AddRange(_hipsBone
     .GetComponentsInChildren<Joint>()
     .Select(j => j.GetComponent<Rigidbody>())
     .Where(rb => rb != null)
     .Distinct()
     .ToArray());
    }

    /// <summary>
    /// јктивирует рагдолл и примен€ет силы
    /// </summary>
    /// <param name="force">сила</param>
    /// <param name="from">от какого направлени€</param>
    public void Knockout(float force, Transform from)
    {

        EnableRagdoll(force, from);
        recoveryCoroutine = context.StartCoroutine(Recover());
    }

    /// <summary>
    /// ѕринудительно останавливает корутину подъЄма
    /// </summary>
    public void ForceStop()
    {
        //IsRecovering = false;

        if (recoveryCoroutine != null)
            context.StopCoroutine(recoveryCoroutine);
    }

    /// <summary>
    ///  опирует локальное положение и вращение каждой кости в соответствующие элементы предоставленного массива.
    /// </summary>
    /// <param name="arr">ћассив костей персонажа</param>
    protected void PopulateBoneTransforms(CharacterBoneTransform[] arr)
    {
        for (int i = 0; i < _bones.Length; i++)
        {
            arr[i].position = _bones[i].localPosition;
            arr[i].rotation = _bones[i].localRotation;
        }
    }

    /// <summary>
    /// ќпредел€ет начальную позу указанного анимационного клипа и заполн€ет предоставленный массив преобразовани€ми костей.
    /// </summary>
    /// <param name="clipName">название клипа</param>
    /// <param name="arr">ћассив костей персонажа</param>
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

    #region Aligning Transform

    /// <summary>
    /// ¬ыравнивает вращение персонажа в соответствии с горизонтальным направлением бедер, 
    /// сохран€€ при этом исходное положение и вращение бедер.
    /// </summary>
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

    /// <summary>
    /// ¬ыравнивает положение объекта в соответствии с тазовой костью, корректиру€ его с учетом вращени€ и уровн€ земли.
    /// </summary>

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

    #endregion

    /// <summary>
    /// ¬озвращает массив костей в зависимости от направлени€ тела
    /// </summary>
    /// <returns></returns>
    protected CharacterBoneTransform[] GetStandUpBoneTransforms()
    {
        return isFacingUp ? _faceupBoneTransforms : _facedownBoneTransforms;
    }

    #region Rigidbody Force

    /// <summary>
    /// ѕримен€ет силу по отношению к рагдолу
    /// </summary>
    /// <param name="force">сила</param>
    /// <param name="from">от какого источника</param>
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
    #endregion


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
        yield return AnimatorUtils.WaitForAnimationEnd(anim,animName, AnimatorParameters.damageLayer);

        InvokeRecover();

    }





}
