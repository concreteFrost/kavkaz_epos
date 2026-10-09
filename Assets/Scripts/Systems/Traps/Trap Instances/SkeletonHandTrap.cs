using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHandTrap : BaseTrap, IDamageColliderActivator, IAttackSource
{
    #region Зависимости: здоровье и попадания

    [Header("Зависимости: здоровье и попадания")]
    [Tooltip("Коллайдер, который наносит урон во время удара.")]
    [SerializeField] private DamageCollider attackCollider;

    [Tooltip("Компонент здоровья этой ловушки.")]
    [SerializeField] private DamagableObject damagable;

    [Tooltip("Тип поверхности для эффекта попадания по ловушке.")]
    [SerializeField] private DamagableSurfaceSO damageImpactType;

    #endregion

    #region Зависимости: анимация и призыв

    [Header("Зависимости: анимация и призыв")]
    [Tooltip("Настройки анимации удара и временного окна его коллайдера.")]
    [SerializeField] private AnimationInfoSO animationInfoSO;

    [Tooltip("Эффект частиц, проигрываемый при активации ловушки.")]
    [SerializeField] private ParticleSystem summonParticle;

    [Tooltip("Задержка перед запуском анимации удара после призыва.")]
    [SerializeField] private float summonTimer = 1f;

    private Coroutine summonCoroutine;

    #endregion

    #region Параметры атаки

    [Header("Параметры атаки")]
    [Tooltip("Время до автоматического сброса ловушки после активации.")]
    [SerializeField, Min(0f)] private float maxResetTimer = 3f;

    [Tooltip("Базовый урон атаки.")]
    [SerializeField] private float baseDamage = 1f;

    [Tooltip("Сила, используемая при расчёте итогового урона.")]
    [SerializeField] private float strength = 100f;

    [Tooltip("Тип баланса, сила воздействия и дополнительные эффекты атаки.")]
    [SerializeField] private DamageData damageData;

    [Tooltip("Типы персонажей, которые не получают урон от этой ловушки.")]
    [SerializeField] private List<CharacterType> targetsToIgnore = new() { CharacterType.Enemy };

    #endregion

    #region Аудио
    [SerializeField] private EventReference ev_attack;
    [SerializeField] private EventReference ev_death;
    [SerializeField] private EventReference ev_soil;
    #endregion

    #region Состояние

    private Animator anim;
    private float currentResetTimer;

    #endregion

    #region Контракт IAttackSource

    /// <summary>Возвращает настройки анимации удара.</summary>
    public AnimationInfoSO AnimationInfo() => animationInfoSO;

    /// <summary>Список типов целей, которым эта ловушка не наносит урон.</summary>
    public List<CharacterType> TargetsToIgnore { get => targetsToIgnore; set => targetsToIgnore = value; }

    /// <summary>Возвращает уникальный идентификатор источника атаки.</summary>
    public int SourceId() => gameObject.GetInstanceID();

    /// <summary>Возвращает трансформ источника атаки.</summary>
    public Transform Source() => transform;

    public event Action<IAttackSource> DamageTaken;

    #endregion

    #region Инициализация и состояние ловушки

    private void Start() => Init();

    public override void Init()
    {
        base.Init();
        anim = GetComponentInChildren<Animator>();
        currentResetTimer = 0f;
        attackCollider?.Init();
        damagable.Init();

        summonParticle.Stop();

        if (summonCoroutine != null)
        {
            StopCoroutine(summonCoroutine);
            summonCoroutine = null;
        }

        damagable.Health.Depleted += OnHealthDepleted;
    }

    private void OnDisable()
    {
        damagable.Health.Depleted -= OnHealthDepleted;
    }

    private void OnHealthDepleted()
    {
        wasActivated = true;
        AudioEventPlayer.Play3DOneShot(ev_death, gameObject, "CharacterVoiceReaction", 4);
        Die();
    }

    private void Update()
    {
        if (!wasActivated || wasDestroyed) return;

        currentResetTimer += Time.deltaTime;
        if (currentResetTimer >= maxResetTimer)
            Deactivate();
    }

    /// <summary>Останавливает активный эффект и закрывает атакующий коллайдер.</summary>
    public override void Deactivate()
    {
        wasActivated = false;
        currentResetTimer = 0f;
        summonParticle.Stop();
        DeactivateDamageCollider();
    }

    /// <summary>Запускает эффект призыва и откладывает анимацию удара.</summary>
    public override void Activate()
    {
        base.Activate();
        summonParticle.Play();
        currentResetTimer = 0f;

        AudioEventPlayer.Play3DOneShot(ev_soil, gameObject);

        if (summonCoroutine == null)
            summonCoroutine = StartCoroutine(SummonCoroutine());
    }

    public override void ResetState()
    {
        base.ResetState();
        summonParticle.Stop();
        damagable.Init();
    }

    /// <summary>Запускает анимацию смерти и останавливает ожидающий призыв.</summary>
    public void Die()
    {
        anim.SetTrigger("Death");
        summonParticle.Stop();
        StopSummonCoroutine();
        Destroy();
    }

    protected override void Destroy()
    {
        attackCollider.DisableCollider();
        damagable.ForceDeath();
        wasDestroyed = true;
    }

    #endregion

    #region Корутина призыва

    private IEnumerator SummonCoroutine()
    {
        yield return new WaitForSeconds(summonTimer);
        anim.SetTrigger("Attack");
        summonCoroutine = null;
    }

    private void StopSummonCoroutine()
    {
        if (summonCoroutine == null) return;

        StopCoroutine(summonCoroutine);
        summonCoroutine = null;
    }

    #endregion

    #region Вызовы из анимации

    /// <summary>Включает атакующий коллайдер в момент удара анимации.</summary>
    public void ActivateDamageCollider()
    {
        damageData.SetFinalDamage(baseDamage, strength);
        attackCollider.EnableCollider(damageData, targetsToIgnore, this);
    }

    /// <summary>Выключает атакующий коллайдер после завершения удара.</summary>
    public void DeactivateDamageCollider()
    {
        attackCollider.DisableCollider();
    }

    public void PlayAttack()
    {
        AudioEventPlayer.Play3DOneShot(ev_attack, transform.gameObject);
    }

    #endregion
}
