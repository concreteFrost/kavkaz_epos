using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class DamagableObject : MonoBehaviour, IDamagable
{

    public CharacterType characterType;

    [SerializeField] private float defaultHealth = 20f;
    [SerializeField] HealthModel Health;

    Color defaultCol;
    MeshRenderer mat;

    [SerializeField] Collider damagableCollider;

    #region IDamagable Contract
    public Collider DamageCollider() => damagableCollider;
    public CharacterType CharacterType { get => characterType; set => characterType = value; }
    public Transform GetAimTransform() => transform;
    public Transform GetOrigin() => transform;
    public bool IsDead { get; set; }
    public string SourceId() => null;
    public bool IsDamaged { get; set; }
    public bool IsKnockedOut {  get; set; } 
    public bool InBlockingWindow { get; set; }
    public bool CanPlayDamagedAnimation { get; set; }   
    //public bool IsDefended {  get; set; } = false;
    //public float DefenceBonus { get; set; } = 0;
    public IShield Protection { get; set; } = null; 

    public event Action<IAttackSource> DamageTaken = null;

    public IUiProvider HealthProviderUI { get; set; }
    public DamagableSurfaceSO ImpactVFX() => null;

    #endregion

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if(damagableCollider == null) return;

        damagableCollider.enabled = !IsDead || !IsKnockedOut;
    }

    public virtual void Init()
    {

        Health = new HealthModel(defaultHealth);

        mat = GetComponent<MeshRenderer>();
        defaultCol = mat.material.color;

        characterType = CharacterType.Object;

        
    }

    public virtual void PerformKnockout(Vector3 source, float impactForce)
    {
        //без имплементации
    }

    public void ToggleDamagableCollider(bool isActive) => damagableCollider.enabled = isActive;

    public void TakeDamage(DamageData damageData,IAttackSource source)
    {
        if (IsDead) return;

        Health.ChangeCurrent(damageData.finalDamage, OperationType.Negative);

        StartCoroutine(DamageCoroutine());

        DamageTaken?.Invoke(source);

        if(Health.Current <= 0)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

    }

    public void TakeMaxDamage()
    {
        Health.Current -= Health.CurrentMax;
    }

    IEnumerator DamageCoroutine()
    {
        

        var col = Color.white;
        var col2 = Color.green;

        float elapsed = 0f;

        while(elapsed < 1f)
        {

            col = Color.Lerp(defaultCol, col2, Mathf.PingPong(elapsed += (Time.deltaTime * 2 / 1),1f));
            mat.material.color = col;   

            elapsed += Time.deltaTime;

            yield return null;
        }

        mat.material.color = defaultCol;

    }

  
}
