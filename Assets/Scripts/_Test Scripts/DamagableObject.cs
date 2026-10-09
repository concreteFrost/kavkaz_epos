using FMODUnity;
using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DamagableObject : MonoBehaviour, IDamagable
{

    public CharacterType characterType;

    [SerializeField] private float defaultHealth = 20f;
    [SerializeField] public HealthModel Health;

    [SerializeField] DamagableSurfaceSO surfaceSO;
    [SerializeField] EventReference ev_damage;

    [SerializeField] Collider damagableCollider;

    #region IDamagable Contract
    public Collider DamageCollider() => damagableCollider;
    public CharacterType CharacterType { get => characterType; set => characterType = value; }
    public Transform GetAimTransform() => transform;
    public Transform GetOrigin() => transform;
    public bool IsDead { get; set; }
    public string SourceId() => null;
    public bool IsDamaged { get; set; } = false;
    public bool IsKnockedOut { get; set; } = false;
    public bool InBlockingWindow { get; set; } = false;
    public bool CanPlayDamagedAnimation { get; set; } = true; 

    public IShield Protection { get; set; } = null; 

    public event Action<IAttackSource> DamageTaken = null;

    public IUiProvider HealthProviderUI { get; set; }
    public DamagableSurfaceSO ImpactVFX() => surfaceSO;

    #endregion


    public virtual void Init()
    {

        Health = new HealthModel(defaultHealth);
        damagableCollider.enabled = true;

    }

    public void ForceDeath()
    {
        Health.ChangeCurrent(Health.CurrentMax, OperationType.Negative);
        Die();
    }

    public virtual void PerformKnockout(Vector3 source, float impactForce)
    {
        //без имплементации
    }

    public void ToggleDamagableCollider(bool isActive) => damagableCollider.enabled = isActive;

    public void TakeDamage(DamageData damageData,IAttackSource source)
    {

        Health.ChangeCurrent(damageData.finalDamage, OperationType.Negative);

        CombatVFXManager.ImpactResolved?.Invoke(ImpactVFX(), GetAimTransform().position, -source.Source().forward);
        AudioEventPlayer.Play3DOneShot(ev_damage, transform.gameObject);

        DamageTaken?.Invoke(source);
    }

    public void TakeMaxDamage()
    {
        Health.Current -= Health.CurrentMax;
    }

    private void Die()
    {
        damagableCollider.enabled = false;
        IsDead = true;
    }

  

  
}
